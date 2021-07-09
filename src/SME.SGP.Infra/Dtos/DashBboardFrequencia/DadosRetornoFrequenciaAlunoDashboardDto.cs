namespace SME.SGP.Infra
{
    public class DadosRetornoFrequenciaAlunoDashboardDto
    {
        public string Descricao { get; set; }
        public long QuantidadePresentes { get; set; }
        public long QuantidadeAusentes { get; set; }
        public long QuantidadeRemotos { get; set; }
        public long TotalAlunos { get; set; }
    }
}
