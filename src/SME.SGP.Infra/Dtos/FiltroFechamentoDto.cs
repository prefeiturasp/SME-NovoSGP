using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroFechamentoDto
    {
        public FiltroFechamentoDto()
        {
            FechamentosBimestres = new List<FechamentoBimestreDto>();
        }

        public long? DreId { get; set; }
        public IEnumerable<FechamentoBimestreDto> FechamentosBimestres { get; set; }
        public long TipoCalendarioId { get; set; }
        public long? UeId { get; set; }
    }
}