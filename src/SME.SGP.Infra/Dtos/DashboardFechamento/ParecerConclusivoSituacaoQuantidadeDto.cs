using SME.SGP.Dominio;

namespace SME.SGP.Infra
{ 
    public class ParecerConclusivoSituacaoQuantidadeDto
    {
        public SituacaoFechamento Situacao { get; set; }
        public int Quantidade { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
        public string AnoTurma { get; set; }
    }
}
