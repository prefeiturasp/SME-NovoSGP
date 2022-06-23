using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaConsolidacaoDashBoardFrequenciaPorUeUseCase : AbstractUseCase, IExecutaConsolidacaoDashBoardFrequenciaPorUeUseCase
    {
        public ExecutaConsolidacaoDashBoardFrequenciaPorUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoPorUeDashBoardFrequencia>();

            var turmasIds = await mediator.Send(new ObterTurmasIdPorUeCodigoEAnoLetivoQuery(filtro.AnoLetivo, filtro.UeCodigo));

            if (turmasIds != null || turmasIds.Any())
            { 
                if(filtro.TipoPeriodo == TipoPeriodoDashboardFrequencia.Diario)
                    foreach (var turmaId in turmasIds)
                        await mediator.Send(new InserirConsolidacaoDiariaDashBoardFrequenciaCommand(filtro.AnoLetivo, filtro.Mes, turmaId));

                if(filtro.TipoPeriodo == TipoPeriodoDashboardFrequencia.Semanal)
                    foreach (var turmaId in turmasIds)
                        await mediator.Send(new InserirConsolidacaoSemanalDashBoardFrequenciaCommand(filtro.AnoLetivo, filtro.Mes, turmaId));

                if (filtro.TipoPeriodo == TipoPeriodoDashboardFrequencia.Mensal)
                    foreach (var turmaId in turmasIds)
                        await mediator.Send(new InserirConsolidacaoMensalDashBoardFrequenciaCommand(filtro.AnoLetivo, filtro.Mes, turmaId));
            }

            return true;
        }
    }
}
