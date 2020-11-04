namespace SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao
{
    public class TotaisAdesaoResultado
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int Codigoturma { get; set; }
        public long TotalUsuariosComCpfInvalidos { get; set; }
        public long TotalUsuariosPrimeiroAcesso { get; set; }
        public long TotalUsuariosSemAppInstalado { get; set; }
        public long TotalUsuariosValidos { get; set; }
    }
}
