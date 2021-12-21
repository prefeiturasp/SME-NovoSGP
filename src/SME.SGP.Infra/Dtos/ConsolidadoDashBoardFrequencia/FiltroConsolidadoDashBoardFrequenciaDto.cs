using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Infra
{
    public class FiltroConsolidadoDashBoardFrequenciaDto
    {
        public long TurmaId { get; set; }
        public DateTime DataAula { get; set; }
        public TipoPeriodoDashboardFrequencia TipoPeriodo { get; set; }
    }
}



