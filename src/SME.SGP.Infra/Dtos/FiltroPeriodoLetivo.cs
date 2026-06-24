namespace SME.SGP.Infra
{
    public class FiltroPeriodoLetivo
    {
        public FiltroPeriodoLetivo(int anoLetivo, bool consideraHistorico = false)
        {
            AnoLetivo = anoLetivo;
            ConsideraHistorico = consideraHistorico;
        }

        public int AnoLetivo { get; set; }
        public bool ConsideraHistorico { get; set; }
    }
}
