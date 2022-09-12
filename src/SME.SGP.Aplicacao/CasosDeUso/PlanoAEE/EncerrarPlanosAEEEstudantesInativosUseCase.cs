using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var processoEncerramento = mensagem?.Mensagem != null ? mensagem.ObterObjetoMensagem<PlanoAEEProcessoEncerramentoAutomaticoDto>() : new PlanoAEEProcessoEncerramentoAutomaticoDto();

            if (!processoEncerramento.ContinuarProcessoEncerradosIndevidamente)
            {
                var planosAtivos = await mediator.Send(new ObterPlanosAEEAtivosQuery());
                var anoLetivo = DateTime.Today.Year;

                if (planosAtivos != null && planosAtivos.Any())
                {
                    foreach (var planoAEE in planosAtivos)
                    {
                        var matriculas = await mediator
                            .Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(planoAEE.AlunoCodigo, anoLetivo, filtrarSituacao: false));

                        var turma = await ObterTurma(planoAEE.TurmaId);

                        if (turma == null)
                            throw new NegocioException($"Não foi localizada a turma com id {planoAEE.TurmaId}.");

                        AlunoPorTurmaResposta ultimaMatricula = null;
                        AlunoPorTurmaResposta registroMatriculaTurmaAnterior = null;

                        if (DeveEncerrarPlano(anoLetivo, planoAEE, matriculas, turma, ultimaMatricula, registroMatriculaTurmaAnterior))
                        {
                            if (ultimaMatricula == null)
                                ultimaMatricula = matriculas.OrderBy(a => a.DataSituacao).LastOrDefault();

                            await EncerrarPlanoAEE(planoAEE, registroMatriculaTurmaAnterior?.SituacaoMatricula ?? ultimaMatricula?.SituacaoMatricula ?? "Inativo", registroMatriculaTurmaAnterior?.DataSituacao ?? ultimaMatricula?.DataSituacao ?? DateTime.Now);
                        }
                    }
                }
            }

            await VerificarPlanosEncerradosIndevidamente(processoEncerramento.PaginaEncerradosIdevidamente);

            return true;
        }

        private bool DeveEncerrarPlano(int anoLetivo, PlanoAEE planoAEE, IEnumerable<AlunoPorTurmaResposta> matriculas, Turma turma, AlunoPorTurmaResposta ultimaMatricula = null, AlunoPorTurmaResposta registroMatriculaTurmaAnterior = null)
        {
            var etapaConcluida = false;
            var transferenciaUe = false;

            if (turma != null && (turma.AnoLetivo < anoLetivo))
                etapaConcluida = DeterminaEtapaConcluida(matriculas, planoAEE.AlunoCodigo, turma, ultimaMatricula);

            if (matriculas.Select(m => m.CodigoTurma).Distinct().Count() > 1)
                transferenciaUe = DeterminaTransferenciaUe(matriculas, registroMatriculaTurmaAnterior, turma);

            return (matriculas != null && matriculas.Any() && !matriculas.Any(a => a.EstaAtivo(DateTime.Today))) || etapaConcluida || transferenciaUe;
        }

        private async Task EncerrarPlanoAEE(PlanoAEE planoAEE, string situacaoMatricula, DateTime dataSituacao)
        {
            unitOfWork.IniciarTransacao();

            try
            {
                planoAEE.Situacao = SituacaoPlanoAEE.EncerradoAutomaticamente;

                await mediator.Send(new PersistirPlanoAEECommand(planoAEE));
                await mediator.Send(new ResolverPendenciaPlanoAEECommand(planoAEE.Id));

                if (await ParametroNotificarPlanosAEE())
                    await NotificarEncerramento(planoAEE, situacaoMatricula, dataSituacao);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao encerrar o plano {planoAEE.Id}.", LogNivel.Critico, LogContexto.WorkerRabbit, excecaoInterna: ex.ToString()));
            }
        }

        private async Task<bool> ParametroNotificarPlanosAEE()
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
            var funcionariosCP = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.CP));
            if (funcionariosCP != null && funcionariosCP.Any())
                return funcionariosCP.Select(f => f.CodigoRF).ToList();

            var funcionariosAD = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.AD));
            if (funcionariosAD != null && funcionariosAD.Any())
                return funcionariosAD.Select(f => f.CodigoRF).ToList();

            var funcionariosDiretor = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.Diretor));
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

        private bool DeterminaEtapaConcluida(IEnumerable<AlunoPorTurmaResposta> matriculas, string alunoCodigo, Turma turma, AlunoPorTurmaResposta ultimaMatricula)
        {
            var matriculasAnoTurma = mediator
                .Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(alunoCodigo, turma.AnoLetivo)).Result;

            var concluiuTurma = matriculasAnoTurma
                .Any(m => m.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);

            if (concluiuTurma)
            {
                var ultimaMatriculaAtual = matriculas
                    .OrderBy(m => m.DataMatricula)
                    .LastOrDefault();

                var turmaAtual = ultimaMatriculaAtual != null ?
                    mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(ultimaMatriculaAtual.CodigoTurma.ToString())).Result : null;

                ultimaMatricula = matriculasAnoTurma
                    .OrderBy(m => m.DataSituacao)
                    .FirstOrDefault(m => m.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido);

                return turma.Ue.CodigoUe != turmaAtual.Ue.CodigoUe &&
                       (turma.EhTurmaInfantil && !turmaAtual.EhTurmaInfantil);
            }

            return false;
        }

        private bool DeterminaTransferenciaUe(IEnumerable<AlunoPorTurmaResposta> matriculas, AlunoPorTurmaResposta registroMatriculaTurmaAnterior, Turma turmaPlano)
        {
            var registroMatriculaMaisRecente = matriculas
                .OrderBy(m => m.DataSituacao)
                .Last();

            registroMatriculaTurmaAnterior = matriculas
                .Where(m => !m.CodigoTurma.Equals(registroMatriculaMaisRecente.CodigoTurma))
                .OrderBy(m => m.DataSituacao)
                .Last();

            return !turmaPlano.CodigoTurma.Equals(registroMatriculaMaisRecente.CodigoTurma.ToString()) &&
                   (registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Transferido ||
                    registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Deslocamento ||
                    registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.TransferidoSED) &&
                   !registroMatriculaTurmaAnterior.CodigoEscola.Equals(registroMatriculaMaisRecente.CodigoEscola);
        }

        private async Task VerificarPlanosEncerradosIndevidamente(int pagina)
        {
            var planosEncerradosAutomaticamente = await mediator.Send(new ObterPlanosAEEEncerradosAutomaticamenteQuery(pagina));

            if (!planosEncerradosAutomaticamente.Any())
                return;

            foreach (var planoAee in planosEncerradosAutomaticamente)
            {
                var turma = await ObterTurma(planoAee.TurmaId);

                if (turma == null)
                    continue;

                var matriculas = await mediator
                    .Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(planoAee.AlunoCodigo, DateTime.Today.Year, filtrarSituacao: false));

                if (!matriculas.Any())
                    continue;

                if (!DeveEncerrarPlano(DateTime.Now.Year, planoAee, matriculas, turma))
                {
                    var ultimaPendencia = await mediator.Send(new ObterUltimaPendenciaPlanoAEEQuery(planoAee.Id));

                    if (!string.IsNullOrEmpty(planoAee.ParecerCoordenacao) && !string.IsNullOrEmpty(planoAee.ParecerPAAI) && planoAee.ResponsavelPaaiId.HasValue)
                    {
                        if (ultimaPendencia != null)
                        {
                            if (ultimaPendencia.Titulo.StartsWith("Plano AEE expirado", StringComparison.InvariantCultureIgnoreCase))
                                planoAee.Situacao = SituacaoPlanoAEE.Expirado;
                            else if (ultimaPendencia.Titulo.StartsWith("Plano AEE devolvido", StringComparison.InvariantCultureIgnoreCase))
                                planoAee.Situacao = SituacaoPlanoAEE.Devolvido;
                            else
                                planoAee.Situacao = SituacaoPlanoAEE.Validado;
                        }
                        else
                            planoAee.Situacao = SituacaoPlanoAEE.Validado;

                        await mediator.Send(new SalvarPlanoAeeSimplificadoCommand(planoAee));
                        continue;
                    }
                    else if (!string.IsNullOrEmpty(planoAee.ParecerCoordenacao) && !string.IsNullOrEmpty(planoAee.ParecerPAAI))
                        planoAee.Situacao = SituacaoPlanoAEE.AtribuicaoPAAI;
                    else if (!string.IsNullOrEmpty(planoAee.ParecerCoordenacao))
                        planoAee.Situacao = SituacaoPlanoAEE.ParecerPAAI;
                    else
                        planoAee.Situacao = SituacaoPlanoAEE.ParecerCP;

                    unitOfWork.IniciarTransacao();

                    try
                    {

                        await mediator.Send(new SalvarPlanoAeeSimplificadoCommand(planoAee));

                        if (ultimaPendencia != null)
                        {
                            ultimaPendencia.Situacao = SituacaoPendencia.Pendente;
                            await mediator.Send(new AlterarSituacaoPendenciaCommand(ultimaPendencia.Id, SituacaoPendencia.Pendente));
                        }
                        else
                            await mediator.Send(new GerarPendenciaValidacaoPlanoAEECommand(planoAee.Id));

                        unitOfWork.PersistirTransacao();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.Rollback();
                        await mediator.Publish(new SalvarLogViaRabbitCommand($"Erro ao desfazer o encerramento do plano {planoAee.Id}", LogNivel.Critico, LogContexto.WorkerRabbit, ex.ToString()));
                    }
                }
            }

            var mensagem = new PlanoAEEProcessoEncerramentoAutomaticoDto(true, pagina += 1);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.EncerrarPlanoAEEEstudantesInativos, mensagem));            
        }
    }
}
