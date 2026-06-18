using SME.SGP.Dominio;

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
