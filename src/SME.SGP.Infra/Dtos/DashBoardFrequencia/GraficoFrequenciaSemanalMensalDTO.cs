using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class GraficoFrequenciaSemanalMensalDTO
    {
        public string Descricao { get; set; }
        public int QuantidadeAcimaMinimoFrequencia { get; set; }

        public int QuantidadeAbaixoMinimoFrequencia { get; set; }        
    }
}
