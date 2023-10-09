using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPARelatorioDinamico
    {
        public long Id { get; set; }
        public string Dre { get; set; }
        public string UnidadeEscolar { get; set; }
        public string Estudante { get; set; }
        public string Ano {  get; set; }
        public Modalidade Modalidade { get; set; }
        public string DescricaoAno { get { return string.Concat(Modalidade.ShortName(), "-", Ano); } }
    }
}
