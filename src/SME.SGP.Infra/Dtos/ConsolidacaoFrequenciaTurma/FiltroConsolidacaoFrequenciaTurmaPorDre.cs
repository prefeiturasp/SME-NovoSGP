using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoFrequenciaTurmaPorDre
    {
        public FiltroConsolidacaoFrequenciaTurmaPorDre(DateTime data, 
                                                       TipoConsolidadoFrequencia tipoConsolidado, 
                                                       long dreId, 
                                                       double percentualMinimo, 
                                                       double percentualMinimoInfantil)
        {
            Data = data;
            TipoConsolidado = tipoConsolidado;
            DreId = dreId;
            PercentualMinimo = percentualMinimo;
            PercentualMinimoInfantil = percentualMinimoInfantil;
        }

        public DateTime Data { get; }
        public TipoConsolidadoFrequencia TipoConsolidado { get; set; }
        public long DreId { get;  }
        public double PercentualMinimo { get; }
        public double PercentualMinimoInfantil { get; }
    }
}
