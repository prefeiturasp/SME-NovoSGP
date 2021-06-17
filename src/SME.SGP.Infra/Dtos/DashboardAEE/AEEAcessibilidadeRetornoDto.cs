using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class AEEAcessibilidadeRetornoDto
    {
        public string Descricao { get; set; }
        public string LegendaSim { get; set; }
        public string LegendaNao { get; set; }
        public int QuantidadeSim { get; set; }
        public int QuantidadeNao { get; set; }
    }
}
