using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FrequenciaGlobalPorAnoDto
    {
        public Modalidade Modalidade { get; set; }
        public int Ano { get; set; }
        public int Quantidade { get; set; }
        public string Descricao { get; set; }
    }
}
