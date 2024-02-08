using System;

namespace SME.SGP.Infra
{
    public class FiltroIdAnoLetivoDto
    {
        public FiltroIdAnoLetivoDto(long id, int anoLetivo, DateTime data)
        {
            Id = id;
            AnoLetivo = anoLetivo;
            Data = data;
        }

        public long Id { get; set; }
        public int AnoLetivo { get; set; }
        public DateTime Data { get; set; }
    }
}
