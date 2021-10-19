using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConsolidarDashBoardFrequenciaCommand : IRequest<bool>
    {
        public IncluirFilaConsolidarDashBoardFrequenciaCommand(long turmaId, DateTime dataAula, TipoPeriodoDashboardFrequencia tipoPeriodo)
        {
            TurmaId = turmaId;
            DataAula = dataAula;
            TipoPeriodo = tipoPeriodo;
        }

        public long TurmaId { get; set; }
        public DateTime DataAula { get; set; }
        public TipoPeriodoDashboardFrequencia TipoPeriodo { get; set; }
    }
}
