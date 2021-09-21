using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class RegistroItineranciaPorAnoDto
    {
        public string Ano { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Quantidade { get; set; }
    }
}
