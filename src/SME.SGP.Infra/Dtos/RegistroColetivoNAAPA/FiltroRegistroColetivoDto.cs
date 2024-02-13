using System;

namespace SME.SGP.Infra
{
    public class FiltroRegistroColetivoDto
    {
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long? UeId { get; set; }
        public DateTime? DataReuniaoInicio { get; set; }
        public DateTime? DataReuniaoFim { get; set; }
        public long[] TiposReuniaoId { get; set; }
    }
}