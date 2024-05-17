using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarPlanosAEEEstudantesInativosTratarUseCase : AbstractUseCase, IEncerrarPlanosAEEEstudantesInativosTratarUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public EncerrarPlanosAEEEstudantesInativosTratarUseCase(IMediator mediator, IUnitOfWork unitOfWork)
           : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var planoAEE = mensagemRabbit.ObterObjetoMensagem<PlanoAEE>();
                var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;

                if (planoAEE.NaoEhNulo())
                {
                    var matriculas = await mediator
                        .Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(planoAEE.AlunoCodigo, anoLetivo, filtrarSituacao: false));

                    var turmaDoPlanoAee = await ObterTurma(planoAEE.TurmaId);

                    if (turmaDoPlanoAee.EhNulo())
                        throw new NegocioException(string.Format(MensagemNegocioEncerramentoAutomaticoPlanoAee.Turma_nao_localizada, planoAEE.TurmaId));

                    if (matriculas.EhNulo() || !matriculas.Any())
                        await ProcessarEncerramentoDadosMatriculasAnosAnteriores(planoAEE, anoLetivo, turmaDoPlanoAee);
                    else
                        await ProcessarEncerramentoDadosMatriculasAnoAtual(planoAEE, anoLetivo, matriculas, turmaDoPlanoAee);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(MensagemNegocioEncerramentoAutomaticoPlanoAee.Falha_ao_encerrar_planos,
                    LogNivel.Critico, LogContexto.WorkerRabbit, observacao: ex.Message, rastreamento: ex.StackTrace, excecaoInterna: ex.ToString(), innerException: ex.InnerException?.ToString()));
                throw;
            }
        }

        private async Task ProcessarEncerramentoDadosMatriculasAnoAtual(PlanoAEE planoAEE, int anoLetivo, IEnumerable<AlunoPorTurmaResposta> matriculas, Turma turmaDoPlanoAee)
        {
            var encerrarPlanoAee = false;
            var ultimaSituacao = matriculas!.Where(m => m.CodigoTipoTurma == (int)TipoTurma.Regular)
                    .OrderByDescending(c => (c.DataSituacao.Ticks, c.DataAtualizacaoTabela.Ticks))
                        .ThenByDescending(c => c.NumeroAlunoChamada)?.FirstOrDefault();

            if (ultimaSituacao.NaoEhNulo())
            {
                if (ultimaSituacao!.Inativo && PlanoDeveSerEncerrado(ultimaSituacao.CodigoSituacaoMatricula))
                    encerrarPlanoAee = true;
                else if (ultimaSituacao!.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido || ultimaSituacao!.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo)
                {
                    if (turmaDoPlanoAee.AnoLetivo <= anoLetivo)
                    {
                        var turmaAtualDoAluno = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(ultimaSituacao.CodigoTurma.ToString()));

                        if (turmaAtualDoAluno.NaoEhNulo())
                        {
                            if (turmaDoPlanoAee.Ue.CodigoUe != turmaAtualDoAluno.Ue.CodigoUe)
                                encerrarPlanoAee = true;
                        }
                    }
                }
                else if (matriculas.Select(m => m.CodigoTurma).Distinct().Count() > 1 && AlunoFoiTransferidoDaUnidadeEscolar(matriculas, turmaDoPlanoAee))
                    encerrarPlanoAee = true;
            }
            else
                encerrarPlanoAee = true;

            if (encerrarPlanoAee || (turmaDoPlanoAee.EhTurmaPrograma() && ultimaSituacao.EhNulo()))
                await EncerrarPlanoAee(planoAEE, ultimaSituacao?.SituacaoMatricula ?? "Inativo", ultimaSituacao?.DataSituacao ?? DateTimeExtension.HorarioBrasilia());
            else if (planoAEE.Situacao == SituacaoPlanoAEE.EncerradoAutomaticamente)
                await ReaverPlanoEncerradoIndevidamente(planoAEE, turmaDoPlanoAee);
        }

        private async Task ProcessarEncerramentoDadosMatriculasAnosAnteriores(PlanoAEE planoAEE, int anoLetivo, Turma turmaDoPlanoAee)
        {
            var uePlanoAluno = turmaDoPlanoAee.Ue.CodigoUe;
            var dadosMatriculaAlunoNaUEPlano = await mediator.Send(new ObterMatriculasAlunoNaUEQuery(uePlanoAluno, planoAEE.AlunoCodigo));

            if (dadosMatriculaAlunoNaUEPlano.NaoEhNulo() && dadosMatriculaAlunoNaUEPlano.Any())
            {
                var situacoesAlunoNaUEAnoAtual = dadosMatriculaAlunoNaUEPlano.Where(a => a.AnoLetivo == anoLetivo);

                if (situacoesAlunoNaUEAnoAtual.Any() && situacoesAlunoNaUEAnoAtual.NaoEhNulo())
                {
                    var codigosTurmasAluno = situacoesAlunoNaUEAnoAtual.Select(b => b.CodigoTurma.ToString()).ToArray();
                    var turmas = await mediator.Send(new ObterTurmasDreUePorCodigosQuery(codigosTurmasAluno));
                    var codigoTurmaRegular = turmas!.Where(b => b.TipoTurma == TipoTurma.Regular).Select(b => long.Parse(b.CodigoTurma)).FirstOrDefault();
                    var ultimaSituacaoAlunoNaUE = situacoesAlunoNaUEAnoAtual.Where(b => b.CodigoTurma == codigoTurmaRegular).OrderByDescending(c => c.DataSituacao).FirstOrDefault();

                    if (ultimaSituacaoAlunoNaUE.NaoEhNulo() && PlanoDeveSerEncerrado(ultimaSituacaoAlunoNaUE.CodigoSituacaoMatricula))
                        await EncerrarPlanoAee(planoAEE, ultimaSituacaoAlunoNaUE?.SituacaoMatricula ?? "Inativo", ultimaSituacaoAlunoNaUE.DataSituacao);
                    else if (planoAEE.Situacao == SituacaoPlanoAEE.EncerradoAutomaticamente)
                        await ReaverPlanoEncerradoIndevidamente(planoAEE, turmaDoPlanoAee);
                }
                else
                {
                    var dadosMatricula = dadosMatriculaAlunoNaUEPlano.Where(x => x.CodigoTurma == long.Parse(turmaDoPlanoAee.CodigoTurma))?.OrderByDescending(c => c.DataSituacao).FirstOrDefault();
                    await EncerrarPlanoAee(planoAEE, dadosMatricula?.SituacaoMatricula ?? "Inativo", dadosMatricula?.DataSituacao ?? DateTimeExtension.HorarioBrasilia());
                }
            }
            else
                throw new NegocioException(string.Format(MensagemNegocioEncerramentoAutomaticoPlanoAee.Nao_foi_localizada_nenhuma_matricula, planoAEE.AlunoCodigo));
        }

        public static bool PlanoDeveSerEncerrado(SituacaoMatriculaAluno situacao)
            => !(new[] { SituacaoMatriculaAluno.Ativo,
                         SituacaoMatriculaAluno.Rematriculado,
                         SituacaoMatriculaAluno.PendenteRematricula,
                         SituacaoMatriculaAluno.SemContinuidade,
                         SituacaoMatriculaAluno.Concluido,
                         SituacaoMatriculaAluno.RemanejadoSaida,
                         SituacaoMatriculaAluno.ReclassificadoSaida,
                         SituacaoMatriculaAluno.Transferido,
                         SituacaoMatriculaAluno.TransferidoSED
                    }).Contains(situacao);
        private async Task EncerrarPlanoAee(PlanoAEE planoAEE, string situacaoMatricula, DateTime dataSituacao)
        {
            if (planoAEE.Situacao == SituacaoPlanoAEE.EncerradoAutomaticamente)
                return;

            try
            {
                unitOfWork.IniciarTransacao();

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
            var parametro = await mediator
                .Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarNotificacaoPlanoAEE, DateTimeExtension.HorarioBrasilia().Year));

            return parametro.NaoEhNulo() && parametro.Ativo;
        }

        private async Task NotificarEncerramento(PlanoAEE plano, string situacaoMatricula, DateTime dataSituacao)
        {
            var turma = await ObterTurma(plano.TurmaId);

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE encerrado automaticamente - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O Plano AEE {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} foi encerrado automaticamente. Motivo: {situacaoMatricula} em {dataSituacao}.";

            var usuarisoIds = await ObterUsuariosParaNotificacao(turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre);

            if (usuarisoIds.NaoEhNulo() && usuarisoIds.Any())
                await mediator.Send(new GerarNotificacaoPlanoAEECommand(plano.Id, usuarisoIds, titulo, descricao, NotificacaoPlanoAEETipo.PlanoExpirado, NotificacaoCategoria.Aviso));
        }

        private async Task<IEnumerable<long>> ObterUsuariosParaNotificacao(string ueCodigo, string dreCodigo)
        {
            var coordenadoresUe = await ObterCoordenadoresUe(ueCodigo);

            var usuariosIds = await ObterUsuariosId(coordenadoresUe);
            var coordenadoresCEFAI = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(dreCodigo));

            if (coordenadoresCEFAI.NaoEhNulo() && coordenadoresCEFAI.Any())
                foreach (var coordenadorCEFAI in coordenadoresCEFAI)
                    usuariosIds.Add(coordenadorCEFAI);

            return usuariosIds;
        }

        private async Task<List<string>> ObterCoordenadoresUe(string codigoUe)
        {
            var funcionariosCP = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.CP));
            if (funcionariosCP.NaoEhNulo() && funcionariosCP.Any())
                return funcionariosCP.Select(f => f.CodigoRF).ToList();

            var funcionariosAD = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.AD));
            if (funcionariosAD.NaoEhNulo() && funcionariosAD.Any())
                return funcionariosAD.Select(f => f.CodigoRF).ToList();

            var funcionariosDiretor = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.Diretor));
            if (funcionariosDiretor.NaoEhNulo() && funcionariosDiretor.Any())
                return funcionariosDiretor.Select(f => f.CodigoRF).ToList();

            return null;
        }

        private async Task<List<long>> ObterUsuariosId(List<string> funcionarios)
        {
            var usuariosIds = new List<long>();

            if (funcionarios.NaoEhNulo())
            {
                foreach (var functionario in funcionarios)
                {
                    var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(functionario));
                    if (usuarioId > 0)
                        usuariosIds.Add(usuarioId);
                }
            }

            return usuariosIds;
        }

        private async Task<Turma> ObterTurma(long turmaId)
            => await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

        private static bool AlunoFoiTransferidoDaUnidadeEscolar(IEnumerable<AlunoPorTurmaResposta> matriculas, Turma turmaPlano)
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
                    registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.TransferidoSED ||
                    (registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Desistente &&
                    !registroMatriculaTurmaAnterior.CodigoEscola.Equals(registroMatriculaMaisRecente.CodigoEscola))) &&
                   !registroMatriculaTurmaAnterior.CodigoEscola.Equals(registroMatriculaMaisRecente.CodigoEscola);
        }

        private async Task ReaverPlanoEncerradoIndevidamente(PlanoAEE planoAEE, Turma turmaPlano)
        {
            try
            {
                unitOfWork.IniciarTransacao();

                planoAEE.Situacao = await DefinirSituacaoPlanoAeeReaver(planoAEE, turmaPlano);

                await mediator.Send(new PersistirPlanoAEECommand(planoAEE));

                if (DeveReaverPendencia(planoAEE.Situacao))
                    await mediator.Send(new ReaverPendenciaPlanoAeeQuery(planoAEE.Id));

                unitOfWork.PersistirTransacao();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                await mediator.Send(new SalvarLogViaRabbitCommand($"Reaver o plano AEE {planoAEE.Id} encerrado indevidamente.", LogNivel.Critico, LogContexto.WorkerRabbit, excecaoInterna: ex.ToString()));
            }
        }

        private async Task<SituacaoPlanoAEE> DefinirSituacaoPlanoAeeReaver(PlanoAEE planoAEE, Turma turmaPlano)
        {
            if (string.IsNullOrWhiteSpace(planoAEE.ParecerCoordenacao))
                return await Task.FromResult(SituacaoPlanoAEE.ParecerCP);

            if (!planoAEE.ResponsavelPaaiId.HasValue)
                return await Task.FromResult(SituacaoPlanoAEE.AtribuicaoPAAI);

            if (string.IsNullOrEmpty(planoAEE.ParecerPAAI))
                return await Task.FromResult(SituacaoPlanoAEE.ParecerPAAI);

            var periodosEscolares = await mediator
                .Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turmaPlano.ModalidadeCodigo, turmaPlano.AnoLetivo, turmaPlano.Semestre));

            var ultimoBimestre = periodosEscolares.LastOrDefault(pe => DateTimeExtension.HorarioBrasilia().Date > pe.PeriodoFim.Date);

            return await Task.FromResult(ultimoBimestre.NaoEhNulo() && (planoAEE.AlteradoEm?.Date ?? DateTimeExtension.HorarioBrasilia().Date) > ultimoBimestre.PeriodoFim.Date ? SituacaoPlanoAEE.Expirado : SituacaoPlanoAEE.Validado);
        }

        private static bool DeveReaverPendencia(SituacaoPlanoAEE situacaoPlanoAEE)
            => new SituacaoPlanoAEE[]
            {
                SituacaoPlanoAEE.ParecerCP,
                SituacaoPlanoAEE.AtribuicaoPAAI,
                SituacaoPlanoAEE.ParecerPAAI
            }.Contains(situacaoPlanoAEE);
    }
}
