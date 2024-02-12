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
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao
{
    public class EncerrarPlanosAEEEstudantesInativosTratarUseCase : AbstractUseCase, IEncerrarPlanosAEEEstudantesInativosTratarUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public EncerrarPlanosAEEEstudantesInativosTratarUseCase(IMediator mediator,
           IUnitOfWork unitOfWork)
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
                //log-------------------------------
                    var logPlanoAee = new LogPlanoAee();
                //----------------------------------

                if (planoAEE.NaoEhNulo())
                {
                    var encerrarPlanoAee = false;

                    var matriculas = await mediator
                        .Send(new ObterMatriculasAlunoPorCodigoEAnoQuery(planoAEE.AlunoCodigo, anoLetivo, filtrarSituacao: false));

                    var turmaDoPlanoAee = await ObterTurma(planoAEE.TurmaId);

                    //log-------------------------------
                        logPlanoAee.matriculas = matriculas;
                        logPlanoAee.turmaDoPlanoAee = turmaDoPlanoAee;
                    //----------------------------------

                    if (turmaDoPlanoAee.EhNulo())
                        throw new NegocioException(string.Format(MensagemNegocioEncerramentoAutomaticoPlanoAee.Turma_nao_localizada, planoAEE.TurmaId));

                    if (matriculas.EhNulo() || !matriculas.Any())
                    {
                        var uePlanoAluno = turmaDoPlanoAee.Ue.CodigoUe;
                        var dadosMatriculaAlunoNaUEPlano = await mediator.Send(new ObterMatriculasAlunoNaUEQuery(uePlanoAluno, planoAEE.AlunoCodigo));

                        //log-------------------------------
                            logPlanoAee.uePlanoAluno = uePlanoAluno;
                            logPlanoAee.dadosMatriculaAlunoNaUEPlano = dadosMatriculaAlunoNaUEPlano;
                        //----------------------------------

                        if (dadosMatriculaAlunoNaUEPlano.NaoEhNulo() && dadosMatriculaAlunoNaUEPlano.Any())
                        {
                            var situacoesAlunoNaUEAnoAtual = dadosMatriculaAlunoNaUEPlano.Where(a => a.AnoLetivo == anoLetivo);

                            //log-------------------------------
                                logPlanoAee.situacoesAlunoNaUEAnoAtual = situacoesAlunoNaUEAnoAtual;
                            //----------------------------------

                            if (situacoesAlunoNaUEAnoAtual.Any() && situacoesAlunoNaUEAnoAtual.NaoEhNulo())
                            {
                                var codigosTurmasAluno = situacoesAlunoNaUEAnoAtual.Select(b => b.CodigoTurma.ToString()).ToArray();

                                var turmas = await mediator.Send(new ObterTurmasDreUePorCodigosQuery(codigosTurmasAluno));

                                var codigoTurmaRegular = turmas!.Where(b => b.TipoTurma == TipoTurma.Regular).Select(b => long.Parse(b.CodigoTurma)).FirstOrDefault();

                                var ultimaSituacaoAlunoNaUE = situacoesAlunoNaUEAnoAtual.Where(b => b.CodigoTurma == codigoTurmaRegular).OrderByDescending(c => c.DataSituacao).FirstOrDefault();

                                //log-------------------------------
                                    logPlanoAee.codigosTurmasAluno = codigosTurmasAluno;
                                    logPlanoAee.ultimaSituacaoAlunoNaUE = ultimaSituacaoAlunoNaUE;
                                    logPlanoAee.turmas = turmas;
                                    logPlanoAee.codigoTurmaRegular = codigoTurmaRegular;
                                    logPlanoAee.planoAEE = planoAEE;
                                    logPlanoAee.situacaoMatricula = ultimaSituacaoAlunoNaUE?.SituacaoMatricula ?? "Inativo";
                                    logPlanoAee.dataSituacao = ultimaSituacaoAlunoNaUE.DataSituacao;
                                    logPlanoAee.planoDeveSerEncerrado = PlanoDeveSerEncerrado(ultimaSituacaoAlunoNaUE.CodigoSituacaoMatricula);
                                //----------------------------------

                                if (ultimaSituacaoAlunoNaUE.NaoEhNulo() && PlanoDeveSerEncerrado(ultimaSituacaoAlunoNaUE.CodigoSituacaoMatricula))
                                    await EncerrarPlanoAee(planoAEE, ultimaSituacaoAlunoNaUE?.SituacaoMatricula ?? "Inativo", ultimaSituacaoAlunoNaUE.DataSituacao);
                            }
                            else
                            {
                                var dadosMatricula = dadosMatriculaAlunoNaUEPlano.Where(x => x.CodigoTurma == long.Parse(turmaDoPlanoAee.CodigoTurma))?.OrderByDescending(c => c.DataSituacao).FirstOrDefault();
                                //log-------------------------------
                                    logPlanoAee.dadosMatricula = dadosMatricula;
                                    logPlanoAee.planoAEE = planoAEE;
                                    logPlanoAee.situacaoMatricula = dadosMatricula?.SituacaoMatricula ?? "Inativo";
                                    logPlanoAee.dataSituacao = dadosMatricula.DataSituacao;
                                //----------------------------------
                                await EncerrarPlanoAee(planoAEE, dadosMatricula?.SituacaoMatricula ?? "Inativo", dadosMatricula.DataSituacao);
                            }
                        }
                        else
                            throw new NegocioException(string.Format(MensagemNegocioEncerramentoAutomaticoPlanoAee.Nao_foi_localizada_nenhuma_matricula, planoAEE.AlunoCodigo));
                    }
                    else
                    {
                        var ultimaSituacao = matriculas!.OrderByDescending(c => c.DataSituacao).ThenByDescending(c => c.NumeroAlunoChamada)?.FirstOrDefault();

                        //log-------------------------------
                            logPlanoAee.ultimaSituacao = ultimaSituacao;
                        //----------------------------------

                        if (ultimaSituacao.NaoEhNulo())
                        {
                            //log-------------------------------
                                logPlanoAee.planoDeveSerEncerrado = PlanoDeveSerEncerrado(ultimaSituacao.CodigoSituacaoMatricula);
                            //----------------------------------

                            if (ultimaSituacao!.Inativo && PlanoDeveSerEncerrado(ultimaSituacao.CodigoSituacaoMatricula))
                                encerrarPlanoAee = true;
                            else if (ultimaSituacao!.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido
                                      || ultimaSituacao!.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo)
                            {
                                //log-------------------------------
                                    logPlanoAee.anoLetivo = anoLetivo;
                                //----------------------------------

                                if (turmaDoPlanoAee.AnoLetivo < anoLetivo)
                                {
                                    var turmaAtualDoAluno = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(ultimaSituacao.CodigoTurma.ToString()));

                                    //log-------------------------------
                                        logPlanoAee.turmaAtualDoAluno = turmaAtualDoAluno;
                                    //----------------------------------

                                    if (turmaDoPlanoAee.Ue.CodigoUe != turmaAtualDoAluno.Ue.CodigoUe)
                                        encerrarPlanoAee = true;
                                }
                            }
                            else if (matriculas.Select(m => m.CodigoTurma).Distinct().Count() > 1 &&
                                     AlunoFoiTransferidoDaUnidadeEscolar(matriculas, turmaDoPlanoAee))
                                encerrarPlanoAee = true;

                            //log-------------------------------
                                logPlanoAee.elseIfQtdMatriculas = matriculas.Select(m => m.CodigoTurma).Distinct().Count() > 1;
                                logPlanoAee.alunoFoiTransferidoDaUnidadeEscolar = AlunoFoiTransferidoDaUnidadeEscolar(matriculas, turmaDoPlanoAee);
                            //----------------------------------
                        }

                        //log-------------------------------
                            logPlanoAee.encerrarPlanoAee = encerrarPlanoAee;
                            logPlanoAee.planoAEE = planoAEE;
                            logPlanoAee.situacaoMatricula = ultimaSituacao?.SituacaoMatricula ?? "Inativo";
                            logPlanoAee.dataSituacao = ultimaSituacao.DataSituacao;
                        //----------------------------------

                        if (encerrarPlanoAee)
                            await EncerrarPlanoAee(planoAEE, ultimaSituacao?.SituacaoMatricula ?? "Inativo", ultimaSituacao.DataSituacao);
                    }

                    await EnviarLog(logPlanoAee); 
                    return true;
                }

                await EnviarLog(logPlanoAee);
                return false;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(MensagemNegocioEncerramentoAutomaticoPlanoAee.Falha_ao_encerrar_planos, LogNivel.Critico, LogContexto.WorkerRabbit, observacao: ex.Message, rastreamento: ex.StackTrace, excecaoInterna: ex.ToString(), innerException: ex.InnerException?.ToString()));
                throw;
            }
        }
        private async Task EnviarLog(LogPlanoAee planoAee)
        {
            if(planoAee.planoAEE.Id == 31449)
            {
                var logPlanoAeeJson = JsonConvert.SerializeObject(planoAee);
                await mediator.Send(new SalvarLogViaRabbitCommand(logPlanoAeeJson, LogNivel.Informacao, LogContexto.WorkerRabbit, rastreamento: "PlanoAEEInfoInconsistente"));
            }
        }
        private class LogPlanoAee
        {
            public IEnumerable<AlunoPorTurmaResposta> matriculas { get; set; }
            public IEnumerable<AlunoPorUeDto> dadosMatriculaAlunoNaUEPlano { get; set; }
            public IEnumerable<AlunoPorUeDto> situacoesAlunoNaUEAnoAtual { get; set; }
            public IEnumerable<Turma> turmas { get; set; }
            public AlunoPorUeDto dadosMatricula { get; set; }
            public Turma turmaDoPlanoAee { get; set; }
            public Turma turmaAtualDoAluno { get; set; }
            public AlunoPorUeDto ultimaSituacaoAlunoNaUE { get; set; }
            public AlunoPorTurmaResposta ultimaSituacao  { get; set; }
            public bool encerrarPlanoAee { get; set; }
            public bool planoDeveSerEncerrado { get; set; }
            public long codigoTurmaRegular { get; set; }
            public string uePlanoAluno { get; set; }
            public string[] codigosTurmasAluno { get; set; }
            public PlanoAEE planoAEE { get; set; }
            public string situacaoMatricula { get; set; }
            public DateTime dataSituacao { get; set; }
            public bool elseIfQtdMatriculas { get; set; }
            public bool alunoFoiTransferidoDaUnidadeEscolar { get; set; }
            public int anoLetivo { get; set; }
        }

        public bool PlanoDeveSerEncerrado(SituacaoMatriculaAluno situacao)
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

            if(funcionarios.NaoEhNulo())
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
                    registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.TransferidoSED ||
                    (registroMatriculaTurmaAnterior.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Desistente &&
                    !registroMatriculaTurmaAnterior.CodigoEscola.Equals(registroMatriculaMaisRecente.CodigoEscola))) &&
                   !registroMatriculaTurmaAnterior.CodigoEscola.Equals(registroMatriculaMaisRecente.CodigoEscola);
        }
    }
}
