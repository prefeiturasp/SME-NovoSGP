using System;

namespace SME.SGP.Infra
{
    public class FiltroIdAnoLetivoDto
    {
        public FiltroIdAnoLetivoDto(long id, DateTime data)
        {
            Id = id;
            Data = data;
        }

        public long Id { get; set; }
        public DateTime Data { get; set; }
    }
}
