using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ModalidadesPorAnoItineranciaProgramaDto
    {
        public Modalidade Modalidade { get; set; }
        public AnoItinerarioPrograma Ano { get; set; }
    }

    public class RetornoModalidadesPorAnoItineranciaProgramaDto
    {
        public string ModalidadeAno { get; set; }
        public int Ano { get; set; }
    }
}
