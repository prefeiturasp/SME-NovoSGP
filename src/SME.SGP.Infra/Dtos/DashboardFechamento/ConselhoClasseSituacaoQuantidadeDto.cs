using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ConselhoClasseSituacaoQuantidadeDto
    {
        public SituacaoConselhoClasse Situacao { get; set; }
        public int Quantidade { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
        public string AnoTurma { get; set; }
    }
}