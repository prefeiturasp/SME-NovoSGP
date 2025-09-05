namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalIndicadorAlfabetizacaoCriticaDto
    {
        public string Posicao { get; set; }
        public string Ue { get; set; }
        public string Dre { get; set; }
        public int TotalAlunosNaoAlfabetizados { get; set; }
        public decimal PercentualTotalAlunos { get; set; }
    }
}
