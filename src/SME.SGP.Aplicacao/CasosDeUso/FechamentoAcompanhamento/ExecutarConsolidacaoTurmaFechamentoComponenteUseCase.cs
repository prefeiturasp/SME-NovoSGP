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

            var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(filtro.TurmaId));

            var fechamento = fechamentos?.FirstOrDefault();

            var atualizarConsolidado = false;

            if (fechamento != null && consolidadoTurmaComponente != null) 
                atualizarConsolidado = consolidadoTurmaComponente.Status != fechamento.ObterStatusFechamento();

            consolidadoTurmaComponente = MapearFechamentoConsolidado(filtro, consolidadoTurmaComponente, fechamento, professoresDaTurma);

            if (consolidadoTurmaComponente.Id == 0 || atualizarConsolidado)
                await repositorioFechamentoConsolidado.SalvarAsync(consolidadoTurmaComponente);

            return true;
        }

        private FechamentoConsolidadoComponenteTurma MapearFechamentoConsolidado(FechamentoConsolidacaoTurmaComponenteBimestreDto filtro, FechamentoConsolidadoComponenteTurma consolidadoTurmaComponente, FechamentoTurmaDisciplina fechamento, IEnumerable<Infra.ProfessorTitularDisciplinaEol> professoresDaTurma)
        {
            var statusFechamento = fechamento != null ? fechamento.ObterStatusFechamento() : StatusFechamento.NaoIniciado;

            if (consolidadoTurmaComponente == null)
            {
                var professorComponente = professoresDaTurma.FirstOrDefault(p => p.DisciplinaId == filtro.ComponenteCurricularId);
               
                consolidadoTurmaComponente = new FechamentoConsolidadoComponenteTurma()
                {
                    Bimestre = filtro.Bimestre,
                    ComponenteCurricularCodigo = filtro.ComponenteCurricularId,
                    DataAtualizacao = DateTime.Now,
                    TurmaId = filtro.TurmaId,
                    ProfessorNome = professorComponente.ProfessorNome,
                    ProfessorRf = professorComponente.ProfessorRf
                };
            }

            consolidadoTurmaComponente.Status = statusFechamento;

            return consolidadoTurmaComponente;
        }
    }
}
