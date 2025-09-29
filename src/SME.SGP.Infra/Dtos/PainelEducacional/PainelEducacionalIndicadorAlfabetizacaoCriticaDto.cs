namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalIndicadorAlfabetizacaoCriticaDto
    {
        public int AnoLetivo { get; set; }
        public string Posicao { get; set; }
        public string Ue { get; set; }
        public string CodigoUe { get; set; }
        public string Dre { get; set; }
        public string CodigoDre { get; set; }
        public long TotalAlunosNaoAlfabetizados { get; set; }
        public decimal PercentualTotalAlunos { get; set; }
    }
}
