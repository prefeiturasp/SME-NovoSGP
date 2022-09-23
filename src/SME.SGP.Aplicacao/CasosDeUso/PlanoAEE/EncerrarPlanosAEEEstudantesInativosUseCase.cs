using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    /// <summary>
    /// Método de encerramento de planos para alunos que estejam inativos e com situação concluída.
    /// </summary>
    public class EncerrarPlanosAEEEstudantesInativosUseCase : AbstractUseCase, IEncerrarPlanosAEEEstudantesInativosUseCase
    {
        private readonly IUnitOfWork unitOfWork;

        public EncerrarPlanosAEEEstudantesInativosUseCase(IMediator mediator,
            IUnitOfWork unitOfWork)
            : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var planosAtivos = await mediator.Send(new ObterPlanosAEEAtivosQuery());
                var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;
                if (planosAtivos != null && planosAtivos.Any())
                {
                    foreach (var planoAEE in planosAtivos)
                    {
                        var encerrarPlanoAee = false;

                        var matriculas = await mediator
                            .Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(planoAEE.AlunoCodigo, anoLetivo, filtrarSituacao: false));

                        if (matriculas == null)
                            throw new NegocioException(string.Format(MensagemNegocioEncerramentoAutomaticoPlanoAee.Nao_foi_localizada_nenhuma_matricula, planoAEE.AlunoCodigo));

                        var turmaDoPlanoAee = await ObterTurma(planoAEE.TurmaId);

                        if (turmaDoPlanoAee == null)
                            throw new NegocioException(string.Format(MensagemNegocioEncerramentoAutomaticoPlanoAee.Turma_nao_localizada, planoAEE.TurmaId));

                        var ultimaSituacao = matriculas!.OrderByDescending(c => c.DataSituacao)?.FirstOrDefault();

                        if (ultimaSituacao!.Inativo)
                            encerrarPlanoAee = true;
                        else if (ultimaSituacao!.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido)
                        {
                            if (turmaDoPlanoAee.AnoLetivo < anoLetivo)
                            {
                                var turmaAtualDoAluno = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(ultimaSituacao.CodigoTurma.ToString()));
                                if (turmaDoPlanoAee.Ue.CodigoUe != turmaAtualDoAluno.Ue.CodigoUe)
                                    encerrarPlanoAee = true;
                            }
                        }
                        else if (matriculas.Select(m => m.CodigoTurma).Distinct().Count() > 1)
                        {
                            if (AlunoFoiTransferidoDaUnidadeEscolar(matriculas, turmaDoPlanoAee))
                                encerrarPlanoAee = true;
                        }

                        if (encerrarPlanoAee)
                            await EncerrarPlanoAee(planoAEE, ultimaSituacao?.SituacaoMatricula ?? "Inativo", ultimaSituacao.DataSituacao);
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                await mediator.Publish(new SalvarLogViaRabbitCommand(MensagemNegocioEncerramentoAutomaticoPlanoAee.Falha_ao_encerrar_planos, LogNivel.Critico, LogContexto.WorkerRabbit, observacao: ex.Message, rastreamento: ex.StackTrace, excecaoInterna: ex.ToString(), innerException: ex.InnerException.ToString()));
                throw;
            }
        }

        private async Task EncerrarPlanoAee(PlanoAEE planoAEE, string situacaoMatricula, DateTime dataSituacao)
        {
            unitOfWork.IniciarTransacao();

            try
            {
                planoAEE.Situacao = SituacaoPlanoAEE.EncerradoAutomaticamente;

                await mediator.Send(new PersistirPlanoAEECommand(planoAEE));
                await mediator.Send(new ResolverPendenciaPlanoAEECommand(planoAEE.Id));

                if (await ParametroNotificarPlanosAee())
                    await NotificarEncerramento(planoAEE, situacaoMatricula, dataSituacao);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao encerrar o plano {planoAEE.Id}.", LogNivel.Critico, LogContexto.WorkerRabbit, excecaoInterna: ex.ToString()));
            }
        }

        private async Task<bool> ParametroNotificarPlanosAee()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarNotificacaoPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }

        private async Task NotificarEncerramento(PlanoAEE plano, string situacaoMatricula, DateTime dataSituacao)
        {
            var turma = await ObterTurma(plano.TurmaId);

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE encerrado automaticamente - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O Plano AEE {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} foi encerrado automaticamente. Motivo: {situacaoMatricula} em {dataSituacao}.";

            var usuarisoIds = await ObterUsuariosParaNotificacao(turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre);

            if (usuarisoIds != null && usuarisoIds.Any())
                await mediator.Send(new GerarNotificacaoPlanoAEECommand(plano.Id, usuarisoIds, titulo, descricao, NotificacaoPlanoAEETipo.PlanoExpirado, NotificacaoCategoria.Aviso));
        }

        private async Task<IEnumerable<long>> ObterUsuariosParaNotificacao(string ueCodigo, string dreCodigo)
        {
            var coordenadoresUe = await ObterCoordenadoresUe(ueCodigo);

            var usuariosIds = await ObterUsuariosId(coordenadoresUe);
            var coordenadoresCEFAI = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(dreCodigo));

            if (coordenadoresCEFAI != null && coordenadoresCEFAI.Any())
                foreach (var coordenadorCEFAI in coordenadoresCEFAI)
                    usuariosIds.Add(coordenadorCEFAI);

            return usuariosIds;
        }

        private async Task<List<string>> ObterCoordenadoresUe(string codigoUe)
        {
            var funcionariosCP = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int) Cargo.CP));
            if (funcionariosCP != null && funcionariosCP.Any())
                return funcionariosCP.Select(f => f.CodigoRF).ToList();

            var funcionariosAD = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int) Cargo.AD));
            if (funcionariosAD != null && funcionariosAD.Any())
                return funcionariosAD.Select(f => f.CodigoRF).ToList();

            var funcionariosDiretor = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int) Cargo.Diretor));
            if (funcionariosDiretor != null && funcionariosDiretor.Any())
                return funcionariosDiretor.Select(f => f.CodigoRF).ToList();

            return null;
        }

        private async Task<List<long>> ObterUsuariosId(List<string> funcionarios)
        {
            var usuariosIds = new List<long>();
            foreach (var functionario in funcionarios)
            {
                var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(functionario));
                if (usuarioId > 0)
                    usuariosIds.Add(usuarioId);
            }

            return usuariosIds;
        }

        private async Task<Turma> ObterTurma(long turmaId)
            => await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

        private bool AlunoFoiTransferidoDaUnidadeEscolar(IEnumerable<AlunoPorTurmaResposta> matriculas, Turma turmaPlano)
        {
            var registroMatriculaMaisRecente = matriculas
                .OrderBy(m => m.DataSituacao)
                .Last();

            var registroMatriculaTurmaAnterior = matriculas
                .Where(m => !m.CodigoTurma.Equals(registroMatriculaMaisRecente.CodigoTurma))
                .OrderBy(m => m.DataSituacao)
                .Last();

            return !turmaPlano.CodigoTurma.Equals(registroMatriculaMaisRecente.CodigoTurma.ToString()) &&
                   (registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Transferido ||
                    registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Deslocamento ||
                    registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.TransferidoSED) &&
                   !registroMatriculaTurmaAnterior.CodigoEscola.Equals(registroMatriculaMaisRecente.CodigoEscola);
        }
    }
}