namespace SME.SGP.Infra
{
    public class FiltroPeriodoLetivo
    {
        public FiltroPeriodoLetivo(int anoLetivo, bool consideraHistorico = false, int periodo = 0)
        {
            AnoLetivo = anoLetivo;
            ConsideraHistorico = consideraHistorico;
            Periodo = periodo;
        }

        public int AnoLetivo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int Periodo { get; set; }
    }
}
