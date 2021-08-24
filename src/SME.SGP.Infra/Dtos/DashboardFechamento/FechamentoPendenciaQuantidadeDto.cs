using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class FechamentoPendenciaQuantidadeDto
    {
        public int Quantidade { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
    }
}