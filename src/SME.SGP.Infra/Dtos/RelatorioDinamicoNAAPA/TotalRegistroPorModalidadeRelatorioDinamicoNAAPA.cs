using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class TotalRegistroPorModalidadeRelatorioDinamicoNAAPA
    {
        public long Total { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
        public string DescricaoAno {  get { return !string.IsNullOrEmpty(Ano) ? string.Concat(Modalidade.ShortName(), "-", Ano) : string.Empty; } }
    }
}
