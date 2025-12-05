using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AtendimentoNAAPARelatorioDinamico
    {
        public long Id { get; set; }
        public string Dre { get; set; }
        public string UnidadeEscolar { get { return $"{TipoEscola.ShortName()} {NomeEscola}"; } }
        public string NomeEscola { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string Estudante { get; set; }
        public string Ano {  get; set; }
        public Modalidade Modalidade { get; set; }
        public string DescricaoAno { get { return string.Concat(Modalidade.ShortName(), "-", Ano); } }
    }
}
