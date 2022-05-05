﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
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
            Turma turma = null;

            if (filtro.PlanoAEEId.HasValue && filtro.PlanoAEEId > 0)
            {
                var entidadePlano = await mediator
                    .Send(new ObterPlanoAEEComTurmaPorIdQuery(filtro.PlanoAEEId.Value));

                var alunoTurma = await mediator
                    .Send(new ObterAlunoPorCodigoEAnoQuery(entidadePlano.AlunoCodigo, entidadePlano.Turma.AnoLetivo));

                if (alunoTurma == null)
                    throw new NegocioException("Aluno não encontrado.");

                var anoLetivo = entidadePlano.Turma.AnoLetivo;

                switch (alunoTurma.CodigoSituacaoMatricula)
                {
                    case SituacaoMatriculaAluno.Ativo:
                    case SituacaoMatriculaAluno.Rematriculado:
                    case SituacaoMatriculaAluno.Concluido:
                        {
                            if (entidadePlano.AlteradoEm?.Year != null)
                                anoLetivo = (int)entidadePlano.AlteradoEm?.Year;
                            break;
                        }
                }

                var alunoPorTurmaResposta = await mediator
                    .Send(new ObterAlunoPorCodigoEolQuery(entidadePlano.AlunoCodigo, anoLetivo, entidadePlano.Turma.AnoLetivo == anoLetivo && entidadePlano.Turma.EhTurmaHistorica, false));

                if (alunoPorTurmaResposta == null && entidadePlano.Situacao == SituacaoPlanoAEE.EncerradoAutomaticamente)
                {
                    alunoPorTurmaResposta = await mediator
                        .Send(new ObterAlunoPorCodigoEolQuery(entidadePlano.AlunoCodigo, entidadePlano.Turma.AnoLetivo, entidadePlano.Turma.EhTurmaHistorica, false));
                }

                if (alunoPorTurmaResposta == null)
                    throw new NegocioException("Aluno não localizado");

                turma = await mediator
                    .Send(new ObterTurmaPorCodigoQuery(alunoPorTurmaResposta.CodigoTurma.ToString()));

                var aluno = new AlunoReduzidoDto()
                {
                    Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno) ? alunoPorTurmaResposta.NomeAluno : alunoPorTurmaResposta.NomeSocialAluno,
                    NumeroAlunoChamada = alunoPorTurmaResposta.NumeroAlunoChamada,
                    DataNascimento = alunoPorTurmaResposta.DataNascimento,
                    DataSituacao = alunoPorTurmaResposta.DataSituacao,
                    CodigoAluno = alunoPorTurmaResposta.CodigoAluno,
                    Situacao = alunoPorTurmaResposta.SituacaoMatricula,
                    TurmaEscola = ObterNomeTurmaFormatado(turma),
                    NomeResponsavel = alunoPorTurmaResposta.NomeResponsavel,
                    TipoResponsavel = alunoPorTurmaResposta.TipoResponsavel,
                    CelularResponsavel = alunoPorTurmaResposta.CelularResponsavel,
                    DataAtualizacaoContato = alunoPorTurmaResposta.DataAtualizacaoContato,
                    EhAtendidoAEE = entidadePlano.Situacao != SituacaoPlanoAEE.Encerrado && entidadePlano.Situacao != SituacaoPlanoAEE.EncerradoAutomaticamente
                };

                plano.Id = filtro.PlanoAEEId.Value;
                plano.Auditoria = (AuditoriaDto)entidadePlano;
                plano.Versoes = await mediator
                    .Send(new ObterVersoesPlanoAEEQuery(filtro.PlanoAEEId.Value));
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
            }

            var questionarioId = await mediator
                .Send(new ObterQuestionarioPlanoAEEIdQuery());

            var ultimaVersaoId = ultimaVersao?.Id ?? 0;

            plano.Questoes = await mediator
                .Send(new ObterQuestoesPlanoAEEPorVersaoQuery(questionarioId, ultimaVersaoId, filtro.TurmaCodigo));

            plano.QuestionarioId = questionarioId;

            if (plano.Situacao != SituacaoPlanoAEE.Encerrado && 
                plano.Situacao != SituacaoPlanoAEE.EncerradoAutomaticamente && 
                turma != null && 
                plano.Questoes != null && 
                plano.Questoes.Any() &&
                turma.AnoLetivo.Equals(DateTime.Today.Year))
            {
                var periodoAtual = await consultasPeriodoEscolar.ObterPeriodoAtualPorModalidade(turma.ModalidadeCodigo);
                plano.Questoes.Single(q => q.TipoQuestao == TipoQuestao.PeriodoEscolar).Resposta.Single().Texto = periodoAtual.Id.ToString();
            }

            return plano;
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
    }
}
