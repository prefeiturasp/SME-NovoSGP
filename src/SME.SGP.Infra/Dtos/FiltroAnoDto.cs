using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FiltroAnoDto
    {
        public FiltroAnoDto() { }
        public FiltroAnoDto(DateTime data, TipoConsolidadoFrequencia tipoConsolidado)
        {
            Data = data;
            TipoConsolidado = tipoConsolidado;
        }

        public DateTime Data { get; set; }
        public TipoConsolidadoFrequencia TipoConsolidado { get; set; }
    }
}
