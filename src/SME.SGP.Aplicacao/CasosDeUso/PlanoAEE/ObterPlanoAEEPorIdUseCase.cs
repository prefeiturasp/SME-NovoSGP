using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEPorIdUseCase : AbstractUseCase, IObterPlanoAEEPorIdUseCase
    {
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        public ObterPlanoAEEPorIdUseCase(IMediator mediator,
                                         IConsultasPeriodoEscolar consultasPeriodoEscolar) : base(mediator)
        {
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
        }

        public async Task<PlanoAEEDto> Executar(FiltroPesquisaQuestoesPorPlanoAEEIdDto filtro)
        {
            var plano = new PlanoAEEDto();
            bool verificaMatriculaAnoVigente = false; 
            bool novaVersao = false;
            var alunoCodigo = 0;

            PlanoAEEVersaoDto ultimaVersao = null;
            Turma turma;

            if (filtro.PlanoAEEId.HasValue && filtro.PlanoAEEId > 0)
            {
                var entidadePlano = await mediator
                    .Send(new ObterPlanoAEEComTurmaPorIdQuery(filtro.PlanoAEEId.Value));

                
                if(entidadePlano.EhNulo())
                    throw new NegocioException("Plano AEE não encontrado");
                
                alunoCodigo = int.Parse(entidadePlano.AlunoCodigo);
                var alunoTurma = await mediator
                    .Send(new ObterAlunoPorCodigoEAnoPlanoAeeQuery(entidadePlano.AlunoCodigo,
                        DateTimeExtension.HorarioBrasilia().Year, true));

                if (alunoTurma.NaoEhNulo())
                    verificaMatriculaAnoVigente = true;

                alunoTurma ??= await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(entidadePlano.AlunoCodigo, entidadePlano.Turma.AnoLetivo, true));

                if (alunoTurma.EhNulo())
                    throw new NegocioException("Aluno não encontrado.");

                var anoLetivo = entidadePlano.Turma.AnoLetivo;

                if (entidadePlano.Turma.TipoTurma == TipoTurma.Programa)
                    entidadePlano = await VerificaTurmaProgramaEAtribuiRegularNoPlano(alunoTurma.CodigoTurma, entidadePlano);

                switch (alunoTurma.CodigoSituacaoMatricula)
                {
                    case SituacaoMatriculaAluno.Ativo:
                    case SituacaoMatriculaAluno.Rematriculado:
                    case SituacaoMatriculaAluno.Concluido:
                        {
                            if (entidadePlano.AlteradoEm.NaoEhNulo())
                                anoLetivo = entidadePlano.AlteradoEm.GetValueOrDefault().Year;
                            break;
                        }
                }

                if (alunoTurma.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido
                    && entidadePlano.Turma.AnoLetivo < DateTimeExtension.HorarioBrasilia().Year
                    && !verificaMatriculaAnoVigente)
                    anoLetivo = entidadePlano.Turma.AnoLetivo;
                else if (verificaMatriculaAnoVigente)
                    anoLetivo = DateTimeExtension.HorarioBrasilia().Year;

                var alunoPorTurmaResposta = await mediator
                    .Send(new ObterAlunoPorCodigoEolQuery(entidadePlano.AlunoCodigo, anoLetivo, anoLetivo != DateTimeExtension.HorarioBrasilia().Year && entidadePlano.Turma.AnoLetivo == anoLetivo && entidadePlano.Turma.EhTurmaHistorica, true, entidadePlano.Turma?.CodigoTurma));


                if (alunoPorTurmaResposta.EhNulo() && entidadePlano.Situacao == SituacaoPlanoAEE.EncerradoAutomaticamente)
                {
                    alunoPorTurmaResposta = await mediator
                        .Send(new ObterAlunoPorCodigoEolQuery(entidadePlano.AlunoCodigo, entidadePlano.Turma.AnoLetivo, entidadePlano.Turma.EhTurmaHistorica, false));
                }
                else if ((alunoPorTurmaResposta.EhNulo() && anoLetivo == DateTimeExtension.HorarioBrasilia().Year) ||
                         !SituacaoAtivaPlanoAEE(entidadePlano.Situacao))
                {
                    alunoPorTurmaResposta = await ChecaSeOAlunoTeveMudancaDeTurmaAnual(entidadePlano.AlunoCodigo, anoLetivo);
                }

                if (alunoPorTurmaResposta.EhNulo())
                {
                    var obterMatriculaAlunoTurmaPlano = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(entidadePlano.Turma.CodigoTurma, entidadePlano.AlunoCodigo));
                    if (obterMatriculaAlunoTurmaPlano.Any() && obterMatriculaAlunoTurmaPlano.NaoEhNulo())
                        alunoPorTurmaResposta = obterMatriculaAlunoTurmaPlano.OrderByDescending(a => a.DataSituacao).FirstOrDefault();
                    else
                        throw new NegocioException("Não foi localizada matrícula ativa para o aluno selecionado.");
                }
                    

                turma = await mediator
                    .Send(new ObterTurmaPorCodigoQuery(alunoPorTurmaResposta.CodigoTurma.ToString()));

                if (turma.TipoTurma == TipoTurma.Programa && entidadePlano.Turma.AnoLetivo == anoLetivo)
                    turma = entidadePlano.Turma;
                var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(new string[]{alunoPorTurmaResposta.CodigoAluno}, turma.AnoLetivo);
                var aluno = new AlunoReduzidoDto()
                {
                    Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno) ? alunoPorTurmaResposta.NomeAluno : alunoPorTurmaResposta.NomeSocialAluno,
                    NumeroAlunoChamada = alunoPorTurmaResposta.ObterNumeroAlunoChamada(),
                    DataNascimento = alunoPorTurmaResposta.DataNascimento,
                    DataSituacao = alunoPorTurmaResposta.DataSituacao,
                    CodigoAluno = alunoPorTurmaResposta.CodigoAluno,
                    CodigoSituacaoMatricula = alunoPorTurmaResposta.CodigoSituacaoMatricula,
                    Situacao = alunoPorTurmaResposta.SituacaoMatricula,
                    TurmaEscola = ObterNomeTurmaFormatado(turma),
                    NomeResponsavel = alunoPorTurmaResposta.NomeResponsavel,
                    TipoResponsavel = alunoPorTurmaResposta.TipoResponsavel,
                    CelularResponsavel = alunoPorTurmaResposta.CelularResponsavel,
                    DataAtualizacaoContato = alunoPorTurmaResposta.DataAtualizacaoContato,
                    EhAtendidoAEE = entidadePlano.Situacao != SituacaoPlanoAEE.Encerrado && entidadePlano.Situacao != SituacaoPlanoAEE.EncerradoAutomaticamente,
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == alunoPorTurmaResposta.CodigoAluno),
                };

                plano.Id = filtro.PlanoAEEId.Value;
                plano.Auditoria = (AuditoriaDto)entidadePlano;
                plano.Versoes = await mediator.Send(new ObterVersoesPlanoAEEQuery(filtro.PlanoAEEId.Value));
                plano.Aluno = aluno;
                plano.Situacao = entidadePlano.Situacao;
                plano.SituacaoDescricao = entidadePlano.Situacao.Name();

                var ue = await mediator
                    .Send(new ObterUeComDrePorIdQuery(turma.UeId));

                plano.Turma = new TurmaAnoDto()
                {
                    Id = turma.Id,
                    Codigo = turma.CodigoTurma,
                    AnoLetivo = turma.AnoLetivo,
                    CodigoUE = ue.CodigoUe
                };

                filtro.TurmaCodigo = turma.CodigoTurma;

                ultimaVersao = plano.Versoes
                    .OrderByDescending(a => a.Numero).First();

                plano.Versoes = plano.Versoes
                    .Where(a => a.Id != ultimaVersao.Id).ToList();

                plano.UltimaVersao = ultimaVersao;
                plano.PodeDevolverPlanoAEE = await PodeDevolverPlanoAEE(entidadePlano.SituacaoPodeDevolverPlanoAEE());
                plano.Responsavel = await ObtenhaResponsavel(entidadePlano.ResponsavelId);
            }
            else
            {
                novaVersao = true;
                plano.Responsavel = await ObterResponsavel();
                turma = await mediator.Send(new ObterTurmaPorCodigoQuery(filtro.TurmaCodigo));
            }

            var questionarioId = await mediator
                .Send(ObterQuestionarioPlanoAEEIdQuery.Instance);

            var ultimaVersaoId = ultimaVersao?.Id ?? 0;

            plano.Questoes = await mediator
                .Send(new ObterQuestoesPlanoAEEPorVersaoQuery(questionarioId, ultimaVersaoId, filtro.TurmaCodigo));

            plano.QuestionarioId = questionarioId;

            var periodoAtual = await consultasPeriodoEscolar.ObterPeriodoAtualPorModalidade(turma.ModalidadeCodigo);

            if (plano.Situacao != SituacaoPlanoAEE.Encerrado &&
                plano.Situacao != SituacaoPlanoAEE.EncerradoAutomaticamente &&
                turma.NaoEhNulo() &&
                plano.Questoes.NaoEhNulo() &&
                plano.Questoes.Any() &&
                turma.AnoLetivo.Equals(DateTimeExtension.HorarioBrasilia().Year) &&
                periodoAtual.NaoEhNulo() && plano.Questoes.Any(x => x.TipoQuestao == TipoQuestao.PeriodoEscolar && x.Resposta.Any()) &&
                VerificaSeUltimaVersaoPlanoEDoAnoAtual(plano))
            {
                plano.Questoes.Single(q => q.TipoQuestao == TipoQuestao.PeriodoEscolar).Resposta.Single().Texto =
                    periodoAtual.Id.ToString();
            }
            else
                CriarRespostaPeriodoEscolarParaPlanoASerCriado(plano, periodoAtual, SituacaoAtivaPlanoAEE(plano.Situacao));

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            plano.PermitirExcluir = PermiteExclusaoPlanoAEE(plano.Situacao, usuarioLogado);

            plano.RegistroCadastradoEmOutraUE = !await VerificarUsuarioLogadoPertenceMesmaUEPlano(usuarioLogado, turma);
            plano.PermitirEncerramentoManual = plano.Id > 0 && PermitirEncerramentoManual(plano);

            await BuscarDadosSrmPaee((filtro.CodigoAluno > 0 ?  filtro.CodigoAluno :alunoCodigo),plano,novaVersao);

            return plano;
        }

        private bool PermitirEncerramentoManual(PlanoAEEDto plano)
            => !(new[] { SituacaoMatriculaAluno.Ativo,
                             SituacaoMatriculaAluno.PendenteRematricula,
                             SituacaoMatriculaAluno.Rematriculado,
                             SituacaoMatriculaAluno.SemContinuidade}.Contains(plano.Aluno.CodigoSituacaoMatricula))

                   || plano.Aluno.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido;


        private async Task<bool> VerificarUsuarioLogadoPertenceMesmaUEPlano(Usuario usuarioLogado, Turma turmaEncaminhamentoAee)
        {
            return await mediator.Send(new VerificarUsuarioLogadoPertenceMesmaUEQuery(usuarioLogado, turmaEncaminhamentoAee));
        }
        
        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }

        private async Task BuscarDadosSrmPaee(long codigoAluno,PlanoAEEDto plano,bool novaVersao)
        {
            if (novaVersao)
            {
                var resposta = new List<RespostaQuestaoDto>();
                var dadoSrm = (await mediator.Send(new ObterDadosSrmPaeeColaborativoEolQuery(codigoAluno))).ToList();

                if (dadoSrm.Count > 0)
                {
                    var json = JsonConvert.SerializeObject(dadoSrm); 
                    resposta.Add(new RespostaQuestaoDto() {Texto = json});
                    
                    plano.Questoes.FirstOrDefault(q => q.TipoQuestao == TipoQuestao.InformacoesSrm)!.Resposta = resposta;
                }
            }
            
        }
        
        public void CriarRespostaPeriodoEscolarParaPlanoASerCriado(PlanoAEEDto plano, PeriodoEscolar periodoAtual, bool planoEstaAtivo)
        {
            if (periodoAtual.EhNulo())
                return;

            if (planoEstaAtivo)
            {
                var questaoPeriodoEscolar = plano.Questoes.Single(q => q.TipoQuestao == TipoQuestao.PeriodoEscolar);
                var resposta = new List<RespostaQuestaoDto> { new() { Texto = periodoAtual.Id.ToString() } };
                questaoPeriodoEscolar.Resposta = resposta;

                var questao = plano.Questoes.FirstOrDefault(q => q.TipoQuestao == TipoQuestao.PeriodoEscolar);

                if (questao.EhNulo())
                    return;

                questao.Resposta = questaoPeriodoEscolar.Resposta;
            }
            
        }

        public bool VerificaSeUltimaVersaoPlanoEDoAnoAtual(PlanoAEEDto plano)
            => plano.Id > 0 ? plano.UltimaVersao.CriadoEm.Year.Equals(DateTime.Now.Year) : true;

        private bool PermiteExclusaoPlanoAEE(SituacaoPlanoAEE situacao, Usuario usuarioLogado)
        {
            var EhProfessor = usuarioLogado.EhProfessor() ||
                                        usuarioLogado.EhProfessorPaee();
            var EhGestor = usuarioLogado.EhGestorEscolar();
            var planoDevolvido = (situacao == SituacaoPlanoAEE.Devolvido);
            var planoAguardandoParecerCoordenacao = (situacao == SituacaoPlanoAEE.ParecerCP);

            return (EhProfessor && planoDevolvido) ||
                   (EhGestor && (planoDevolvido || planoAguardandoParecerCoordenacao));
        }

        private async Task<AlunoPorTurmaResposta> ChecaSeOAlunoTeveMudancaDeTurmaAnual(string codigoAluno, int anoLetivo)
        {
            var turmasAluno = await mediator.Send(new ObterTurmasAlunoPorFiltroQuery(codigoAluno, anoLetivo, false, true));
            
            if (turmasAluno.Any())
            {
                var alunoComMatriculaAtiva = turmasAluno.FirstOrDefault(t => t.PossuiSituacaoAtiva());

                if (alunoComMatriculaAtiva.EhNulo())
                    return null;

                return await mediator
                    .Send(new ObterAlunoPorCodigoEolQuery(codigoAluno, anoLetivo, false, false, alunoComMatriculaAtiva.CodigoTurma.ToString()));
            }

            return null;
        }

        public async Task<PlanoAEE> VerificaTurmaProgramaEAtribuiRegularNoPlano(string codigoTurma, PlanoAEE entidadePlano)
        {
            bool turmaAtualizada = false;
            var turmaAlunoEol = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turmaAlunoEol.EhTurmaRegular())
                turmaAtualizada = await mediator.Send(new AtualizarTurmaAlunoPlanoAEECommand() { PlanoAEEId = entidadePlano.Id, TurmaId = turmaAlunoEol.Id });

            if (turmaAtualizada)
                return await mediator.Send(new ObterPlanoAEEComTurmaPorIdQuery(entidadePlano.Id));

            return entidadePlano;
        }

        private bool SituacaoAtivaPlanoAEE(SituacaoPlanoAEE situacaoEntidadePlanoAEE)
        {
            return situacaoEntidadePlanoAEE != SituacaoPlanoAEE.Encerrado
                && situacaoEntidadePlanoAEE != SituacaoPlanoAEE.EncerradoAutomaticamente
                && situacaoEntidadePlanoAEE != SituacaoPlanoAEE.Expirado;
        }

        private string ObterNomeTurmaFormatado(Turma turma)
        {
            return turma.NaoEhNulo() ? turma.NomeComModalidade() : string.Empty;
        }

        private async Task<bool> PodeDevolverPlanoAEE(bool situacaoPodeDevolverPlanoAEE)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuario.EhNulo())
                throw new NegocioException("Usuário não localizado");

            return usuario.EhPerfilProfessor() || !situacaoPodeDevolverPlanoAEE ? false : true;
        }

        private async Task<ResponsavelDto> ObtenhaResponsavel(long id)
        {
            var responsavel = new ResponsavelDto();

            var usuario = await mediator.Send(new ObterUsuarioPorIdSemPerfilQuery(id));

            if (usuario.NaoEhNulo())
            {
                responsavel.ResponsavelId = usuario.Id;
                responsavel.ResponsavelRF = usuario.CodigoRf;
                responsavel.ResponsavelNome = usuario.Nome;
            }

            return responsavel;
        }

        private async Task<ResponsavelDto> ObterResponsavel()
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            return new ResponsavelDto()
            {
                ResponsavelId = usuario.Id,
                ResponsavelRF = usuario.CodigoRf,
                ResponsavelNome = usuario.Nome
            };
        }
    }
}
