using MediatR;
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

            PlanoAEEVersaoDto ultimaVersao = null;
            Turma turma;

            if (filtro.PlanoAEEId.HasValue && filtro.PlanoAEEId > 0)
            {
                var entidadePlano = await mediator
                    .Send(new ObterPlanoAEEComTurmaPorIdQuery(filtro.PlanoAEEId.Value));

                var alunoTurma = await mediator
                    .Send(new ObterAlunoPorCodigoEAnoQuery(entidadePlano.AlunoCodigo, DateTimeExtension.HorarioBrasilia().Year, true));

                if (alunoTurma == null)
                {
                    alunoTurma = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(entidadePlano.AlunoCodigo, entidadePlano.Turma.AnoLetivo, true));
                }

                if (alunoTurma == null)
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
                            if (entidadePlano.AlteradoEm != null)
                                anoLetivo = entidadePlano.AlteradoEm.GetValueOrDefault().Year;
                            break;
                        }
                }

                if (alunoTurma.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido && entidadePlano.Turma.AnoLetivo < DateTimeExtension.HorarioBrasilia().Year && SituacaoAtivaPlanoAEE(entidadePlano))
                    anoLetivo = entidadePlano.Turma.AnoLetivo;

                var alunoPorTurmaResposta = await mediator
                    .Send(new ObterAlunoPorCodigoEolQuery(entidadePlano.AlunoCodigo, anoLetivo, anoLetivo != DateTimeExtension.HorarioBrasilia().Year && entidadePlano.Turma.AnoLetivo == anoLetivo && entidadePlano.Turma.EhTurmaHistorica, true, entidadePlano.Turma?.CodigoTurma));


                if (alunoPorTurmaResposta == null && entidadePlano.Situacao == SituacaoPlanoAEE.EncerradoAutomaticamente)
                {
                    alunoPorTurmaResposta = await mediator
                        .Send(new ObterAlunoPorCodigoEolQuery(entidadePlano.AlunoCodigo, entidadePlano.Turma.AnoLetivo, entidadePlano.Turma.EhTurmaHistorica, false));
                }
                else if ((alunoPorTurmaResposta == null && anoLetivo == DateTimeExtension.HorarioBrasilia().Year) ||
                         !SituacaoAtivaPlanoAEE(entidadePlano))
                {
                    alunoPorTurmaResposta = await ChecaSeOAlunoTeveMudancaDeTurmaAnual(entidadePlano.AlunoCodigo, anoLetivo);
                }

                if (alunoPorTurmaResposta == null)
                    throw new NegocioException("Aluno não localizado");

                turma = await mediator
                    .Send(new ObterTurmaPorCodigoQuery(alunoPorTurmaResposta.CodigoTurma.ToString()));

                if (turma.TipoTurma == TipoTurma.Programa && entidadePlano.Turma.AnoLetivo == anoLetivo)
                    turma = entidadePlano.Turma;

                var aluno = new AlunoReduzidoDto()
                {
                    Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno) ? alunoPorTurmaResposta.NomeAluno : alunoPorTurmaResposta.NomeSocialAluno,
                    NumeroAlunoChamada = alunoPorTurmaResposta.ObterNumeroAlunoChamada(),
                    DataNascimento = alunoPorTurmaResposta.DataNascimento,
                    DataSituacao = alunoPorTurmaResposta.DataSituacao,
                    CodigoAluno = alunoPorTurmaResposta.CodigoAluno,
                    Situacao = alunoPorTurmaResposta.SituacaoMatricula,
                    TurmaEscola = ObterNomeTurmaFormatado(turma),
                    NomeResponsavel = alunoPorTurmaResposta.NomeResponsavel,
                    TipoResponsavel = alunoPorTurmaResposta.TipoResponsavel,
                    CelularResponsavel = alunoPorTurmaResposta.CelularResponsavel,
                    DataAtualizacaoContato = alunoPorTurmaResposta.DataAtualizacaoContato,
                    EhAtendidoAEE = entidadePlano.Situacao != SituacaoPlanoAEE.Encerrado && entidadePlano.Situacao != SituacaoPlanoAEE.EncerradoAutomaticamente,
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
                plano.Responsavel = await ObtenhaResponsavel();
                turma = await mediator.Send(new ObterTurmaPorCodigoQuery(filtro.TurmaCodigo));
            }

            var questionarioId = await mediator
                .Send(new ObterQuestionarioPlanoAEEIdQuery());

            var ultimaVersaoId = ultimaVersao?.Id ?? 0;

            plano.Questoes = await mediator
                .Send(new ObterQuestoesPlanoAEEPorVersaoQuery(questionarioId, ultimaVersaoId, filtro.TurmaCodigo));

            plano.QuestionarioId = questionarioId;

            var periodoAtual = await consultasPeriodoEscolar.ObterPeriodoAtualPorModalidade(turma.ModalidadeCodigo);

            if (plano.Situacao != SituacaoPlanoAEE.Encerrado &&
                plano.Situacao != SituacaoPlanoAEE.EncerradoAutomaticamente &&
                turma != null &&
                plano.Questoes != null &&
                plano.Questoes.Any() &&
                turma.AnoLetivo.Equals(DateTimeExtension.HorarioBrasilia().Year) &&
                periodoAtual != null && plano.Questoes.Any(x => x.TipoQuestao == TipoQuestao.PeriodoEscolar && x.Resposta.Any()) &&
                VerificaSeUltimaVersaoPlanoEDoAnoAtual(plano))
            {
                plano.Questoes.Single(q => q.TipoQuestao == TipoQuestao.PeriodoEscolar).Resposta.Single().Texto =
                    periodoAtual.Id.ToString();
            }
            else
                CriarRespostaPeriodoEscolarParaPlanoASerCriado(plano, periodoAtual);

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            plano.PermitirExcluir = PermiteExclusaoPlanoAEE(plano.Situacao, usuarioLogado);

            return plano;
        }

        private void CriarRespostaPeriodoEscolarParaPlanoASerCriado(PlanoAEEDto plano, PeriodoEscolar periodoAtual)
        {
            if (periodoAtual == null)
                return;
            
            var questaoPeriodoEscolar = plano.Questoes.Single(q => q.TipoQuestao == TipoQuestao.PeriodoEscolar);
            var resposta = new List<RespostaQuestaoDto> { new() { Texto = periodoAtual.Id.ToString() } };
            questaoPeriodoEscolar.Resposta = resposta;

            var questao = plano.Questoes.FirstOrDefault(q => q.TipoQuestao == TipoQuestao.PeriodoEscolar);

            if (questao == null)
                return;
            
            questao.Resposta = questaoPeriodoEscolar.Resposta;
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
                if (turmasAluno.Count() > 0)
                {
                    var alunoComMatriculaAtiva = turmasAluno.Where(t => t.PossuiSituacaoAtiva()).FirstOrDefault();

                    return await mediator
                        .Send(new ObterAlunoPorCodigoEolQuery(codigoAluno, anoLetivo, false, false, alunoComMatriculaAtiva.CodigoTurma.ToString()));
                }
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

        private bool SituacaoAtivaPlanoAEE(PlanoAEE entidadePlano)
        {
            return entidadePlano.Situacao != SituacaoPlanoAEE.Encerrado
                && entidadePlano.Situacao != SituacaoPlanoAEE.EncerradoAutomaticamente
                && entidadePlano.Situacao != SituacaoPlanoAEE.Expirado;
        }

        private string ObterNomeTurmaFormatado(Turma turma)
        {
            return turma != null ? turma.NomeComModalidade() : string.Empty;
        }

        private async Task<bool> PodeDevolverPlanoAEE(bool situacaoPodeDevolverPlanoAEE)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuario == null)
                throw new NegocioException("Usuário não localizado");

            return usuario.EhPerfilProfessor() || !situacaoPodeDevolverPlanoAEE ? false : true;
        }

        private async Task<ResponsavelDto> ObtenhaResponsavel(long id)
        {
            var responsavel = new ResponsavelDto();

            var usuario = await mediator.Send(new ObterUsuarioPorIdSemPerfilQuery(id));

            if (usuario != null)
            {
                responsavel.ResponsavelId = usuario.Id;
                responsavel.ResponsavelRF = usuario.CodigoRf;
                responsavel.ResponsavelNome = usuario.Nome;
            }

            return responsavel;
        }

        private async Task<ResponsavelDto> ObtenhaResponsavel()
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            return new ResponsavelDto()
            {
                ResponsavelId = usuario.Id,
                ResponsavelRF = usuario.CodigoRf,
                ResponsavelNome = usuario.Nome
            };
        }
    }
}
