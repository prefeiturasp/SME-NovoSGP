using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class FechamentoSituacaoQuantidadeDto
    {
        public int Situacao { get; set; }
        public int Quantidade { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Ano { get; set; }
    }
}