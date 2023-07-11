namespace SME.SGP.Infra
{
    public class PeriodosPAPDto
    {
        private const string SEMESTRE = "S";
        public long ConfiguracaoId { get; set; }
        public long PeridoRelatorioId { get; set; }
        public string TipoPeriodicidade { get; set; }
        public string DescricaoTipoPeriodicidade { get { return TipoPeriodicidade == SEMESTRE ? "Semestre" : "Bimestre"; } }
        public int Periodo { get; set; }
    }
}
