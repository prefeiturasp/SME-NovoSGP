using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaFechamentoComponenteUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaFechamentoComponenteUseCase
    {
        private readonly IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado;

        public ExecutarConsolidacaoTurmaFechamentoComponenteUseCase(IMediator mediator, IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado) : base(mediator)
        {
            this.repositorioFechamentoConsolidado = repositorioFechamentoConsolidado ?? throw new ArgumentNullException(nameof(repositorioFechamentoConsolidado));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FechamentoConsolidacaoTurmaComponenteBimestreDto>();

            if (filtro == null)
            {
                SentrySdk.CaptureMessage($"Não foi possível iniciar a consolidação do fechamento da turma -> componente. O id da turma bimestre componente curricular não foram informados", Sentry.Protocol.SentryLevel.Error);
                return false;
            }

            var consolidadoTurmaComponente = await repositorioFechamentoConsolidado.ObterFechamentoConsolidadoPorTurmaBimestreComponenteCurricularAsync(filtro.TurmaId, filtro.ComponenteCurricularId, filtro.Bimestre);

            var fechamentos = await mediator.Send(new ObterFechamentosTurmaComponentesQuery(filtro.TurmaId, new long[] { filtro.ComponenteCurricularId }, filtro.Bimestre));

            if (fechamentos == null || !fechamentos.Any())
            {
                SentrySdk.CaptureMessage($"Não foi possível iniciar a consolidação do fechamento da turma -> componente. Não foram encontrados fechamentos para a turma bimestre componente curricular não foram informados", Sentry.Protocol.SentryLevel.Error);
                return false;
            }

            var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(filtro.TurmaId));

            var fechamento = fechamentos.FirstOrDefault();

            consolidadoTurmaComponente = MapearFechamentoConsolidado(consolidadoTurmaComponente, fechamento, professoresDaTurma);

            if (consolidadoTurmaComponente.Id == 0 || consolidadoTurmaComponente.Status != fechamento.ObterStatusFechamento())
                await repositorioFechamentoConsolidado.SalvarAsync(consolidadoTurmaComponente);

            return true;
        }

        private FechamentoConsolidadoComponenteTurma MapearFechamentoConsolidado(FechamentoConsolidadoComponenteTurma consolidadoTurmaComponente, FechamentoTurmaDisciplina fechamento, IEnumerable<Infra.ProfessorTitularDisciplinaEol> professoresDaTurma)
        {
            if (consolidadoTurmaComponente == null)
            {
                var professorComponente = professoresDaTurma.FirstOrDefault(p => p.DisciplinaId == fechamento.DisciplinaId);

                var bimestre = fechamento.FechamentoTurma.PeriodoEscolar != null ?
                               fechamento.FechamentoTurma.PeriodoEscolar.Bimestre : 0;

                consolidadoTurmaComponente = new FechamentoConsolidadoComponenteTurma()
                {
                    Bimestre = bimestre,
                    ComponenteCurricularCodigo = fechamento.DisciplinaId,
                    DataAtualizacao = DateTime.Now,
                    TurmaId = fechamento.FechamentoTurma.TurmaId,
                    Status = fechamento.ObterStatusFechamento(),
                    ProfessorNome = professorComponente.ProfessorNome,
                    ProfessorRf = professorComponente.ProfessorRf
                };
            }

            consolidadoTurmaComponente.Status = fechamento.ObterStatusFechamento();

            return consolidadoTurmaComponente;
        }
    }
}
