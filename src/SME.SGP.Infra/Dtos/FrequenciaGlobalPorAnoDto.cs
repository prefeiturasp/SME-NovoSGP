using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FrequenciaGlobalPorAnoDto
    {
        public string NomeTurma { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
        public int QuantidadeAcimaMinimoFrequencia { get; set; }
        public int QuantidadeAbaixoMinimoFrequencia { get; set; }
    }
}
