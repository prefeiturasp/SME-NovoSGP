namespace SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao
{
    public class TotaisAdesaoAgrupadoProDreResultado
    {
        public string NomeCompletoDre { get; set; }
        public long TotalUsuariosComCpfInvalidos { get; set; }
        public long TotalUsuariosPrimeiroAcessoIncompleto { get; set; }
        public long TotalUsuariosSemAppInstalado { get; set; }
        public long TotalUsuariosValidos { get; set; }
    }
}
