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
        public bool PeriodoAberto { get; set; }

        private string ObterDescricaoPeriodo()
        {
            if (PeriodoRelatorioPAP == 0 || string.IsNullOrEmpty(TipoConfiguracaoPeriodicaRelatorioPAP))
                return string.Empty;

            var periodo = TipoConfiguracaoPeriodicaRelatorioPAP == SEMESTRE ? "Semestre" : "Bimestre";

            return PeriodoRelatorioPAP + "º " + periodo;
        }
    }
}
