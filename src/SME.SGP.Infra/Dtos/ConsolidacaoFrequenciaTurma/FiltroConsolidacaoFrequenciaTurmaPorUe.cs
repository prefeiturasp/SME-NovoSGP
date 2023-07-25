using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoFrequenciaTurmaPorUe
    {
        public FiltroConsolidacaoFrequenciaTurmaPorUe(DateTime data,
                                                      TipoConsolidadoFrequencia tipoConsolidado, 
                                                      long ueId, 
                                                      double percentualMinimo, 
                                                      double percentualMinimoInfantil)
        {
            Data = data;
            TipoConsolidado = tipoConsolidado;
            UeId = ueId;
            PercentualMinimo = percentualMinimo;
            PercentualMinimoInfantil = percentualMinimoInfantil;
        }

        public DateTime Data { get; }
        public TipoConsolidadoFrequencia TipoConsolidado { get; set; }
        public long UeId { get; set; }
        public double PercentualMinimo { get; }
        public double PercentualMinimoInfantil { get; }
    }
}
