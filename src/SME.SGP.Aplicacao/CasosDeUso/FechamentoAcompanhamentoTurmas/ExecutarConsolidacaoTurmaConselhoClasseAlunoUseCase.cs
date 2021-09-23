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

            SituacaoConselhoClasse statusNovo = SituacaoConselhoClasse.NaoIniciado;

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

                if (filtro.Bimestre == 0)
                {
                    var fechamento = await mediator.Send(new ObterFechamentoPorTurmaPeriodoQuery() { TurmaId = filtro.TurmaId });
                    var conselhoClasse = await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamento.Id));
                    var conselhoClasseAluno = await mediator.Send(new ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery(conselhoClasse.Id, filtro.AlunoCodigo));
                    consolidadoTurmaAluno.ParecerConclusivoId = conselhoClasseAluno != null ? conselhoClasseAluno.ConselhoClasseParecerId : null;
                }


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

                var componentesComNotaFechamentoOuConselho = await mediator.Send(new ObterComponentesComNotaDeFechamentoOuConselhoQuery(turma.AnoLetivo, filtro.TurmaId, filtro.Bimestre, filtro.AlunoCodigo));
                var componentesDaTurmaEol = await mediator.Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(turmasCodigos));
               


                var possuiComponentesSemNotaConceito = componentesDaTurmaEol.Where(x => x.LancaNota == true).Select(x => x.Codigo).ToArray().Except(componentesComNotaFechamentoOuConselho.Select(x=>x.Codigo).ToArray()).Any();

                if (possuiComponentesSemNotaConceito)
                    statusNovo = SituacaoConselhoClasse.EmAndamento;
                else
                    statusNovo = SituacaoConselhoClasse.Concluido;
            }

            if (consolidadoTurmaAluno.ParecerConclusivoId != null)
                statusNovo = SituacaoConselhoClasse.Concluido;

            consolidadoTurmaAluno.Status = statusNovo;

            consolidadoTurmaAluno.DataAtualizacao = DateTime.Now;

            await repositorioConselhoClasseConsolidado.SalvarAsync(consolidadoTurmaAluno);

            return true;
        }
    }
}
