namespace SME.SGP.Infra
{
    public class PeriodosPAPDto
    {
        private const string SEMESTRE = "S";
        public long ConfiguracaoPeriodicaRelatorioPAPId { get; set; }
        public long PeriodoRelatorioPAPId { get; set; }
        public string TipoConfiguracaoPeriodicaRelatorioPAP { get; set; }
        public string DescricaoPeriodo { get { return ObterDescricaoPeriodo(); } }
        public int PeriodoRelatorioPAP { get; set; }

        private string ObterDescricaoPeriodo()
        {
            return PeriodoRelatorioPAP + "º " + TipoConfiguracaoPeriodicaRelatorioPAP == SEMESTRE ? "Semestre" : "Bimestre";
        }
    }
}
