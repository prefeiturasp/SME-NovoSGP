using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ModalidadesPorAnoDto 
    {
        public Modalidade Modalidade { get; set; }
        public int Ano { get; set; }
    }

    public class RetornoModalidadesPorAnoDto
    {
        public string ModalidadeAno { get; set; }
        public int Ano { get; set; }
    }
}
