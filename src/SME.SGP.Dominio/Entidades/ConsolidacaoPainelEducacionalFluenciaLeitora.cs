namespace SME.SGP.Dominio.Entidades
{
    public class ConsolidacaoPainelEducacionalFluenciaLeitora : EntidadeBase
    {
        public long Id { get; set; }
        public string Fluencia { get; set; }
        public string DescricaoFluencia { get; set; }
        public string DreCodigo { get; set; }
        public decimal Percentual { get; set; }
        public int QuantidadeAlunos { get; set; }
        public int Ano { get; set; }
        public int Periodo { get; set; }
    }
}
