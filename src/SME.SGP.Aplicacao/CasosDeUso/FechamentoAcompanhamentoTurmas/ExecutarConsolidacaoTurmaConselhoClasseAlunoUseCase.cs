using MediatR;
using Sentry;
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
    public class ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase
    {
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;

        public ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase(IMediator mediator, IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado) : base(mediator)
        {
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidacaoConselhoClasseAlunoDto>();

            if (filtro == null)
            {
                SentrySdk.CaptureMessage($"Não foi possível iniciar a consolidação do conselho de clase da turma -> aluno. O id da turma bimestre aluno não foram informados", Sentry.Protocol.SentryLevel.Error);
                return false;
            }

            StatusFechamento statusNovo = StatusFechamento.NaoIniciado;

            var consolidadoTurmaAluno = await repositorioConselhoClasseConsolidado.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(filtro.TurmaId, filtro.Bimestre, filtro.AlunoCodigo);
            if (consolidadoTurmaAluno == null)
            {
                consolidadoTurmaAluno = new ConselhoClasseConsolidadoTurmaAluno();
                consolidadoTurmaAluno.AlunoCodigo = filtro.AlunoCodigo;
                consolidadoTurmaAluno.Bimestre = filtro.Bimestre;
                consolidadoTurmaAluno.TurmaId = filtro.TurmaId;
                consolidadoTurmaAluno.Status = statusNovo;
            }

            var componentesDoAluno = await mediator.Send(new ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery(filtro.AlunoCodigo, filtro.Bimestre, filtro.TurmaId));
            if (componentesDoAluno != null && componentesDoAluno.Any())
            {
                var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));
                string[] turmasCodigos;
                if (turma.DeveVerificarRegraRegulares())
                {
                    List<TipoTurma> turmasCodigosParaConsulta = new List<TipoTurma>() { turma.TipoTurma };
                    turmasCodigosParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                    turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, filtro.AlunoCodigo, turmasCodigosParaConsulta));
                }
                else
                {
                    turmasCodigos = new string[1] { turma.CodigoTurma };
                }

                var componentesDaTurmaEol = await mediator.Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(turmasCodigos));
                var componentesDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(componentesDaTurmaEol.Select(x => Convert.ToInt64(x.Codigo)).Distinct().ToArray()));

                var possuiComponentesSemNotaConceito = componentesDaTurma.Select(a => a.CodigoComponenteCurricular).Except(componentesDoAluno).Any();

                if (possuiComponentesSemNotaConceito)
                    statusNovo = StatusFechamento.EmAndamento;
                else
                    statusNovo = StatusFechamento.Concluido;
            }

            if (consolidadoTurmaAluno.Id == 0 || consolidadoTurmaAluno.Status != statusNovo)
            {
                consolidadoTurmaAluno.Status = statusNovo;
                await repositorioConselhoClasseConsolidado.SalvarAsync(consolidadoTurmaAluno);
            }

            return true;
        }
    }
}
