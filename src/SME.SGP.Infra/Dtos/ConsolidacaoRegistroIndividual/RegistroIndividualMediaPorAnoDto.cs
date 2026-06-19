using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class RegistroIndividualMediaPorAnoDto
    {
        public string Ano { get; set; }
        public Modalidade Modalidade { get; set; }
        public double Quantidade { get; set; }
    }
}
