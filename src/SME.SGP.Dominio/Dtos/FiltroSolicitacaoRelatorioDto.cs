namespace SME.SGP.Dominio.Dtos
{
    public class FiltroSolicitacaoRelatorioDto
    {
        public string FiltrosUsados { get; set; }
        public TipoRelatorio Relatorio { get; set; }
        public TipoFormatoRelatorio ExtensaoRelatorio { get; set; }
        public string UsuarioQueSolicitou { get; set; }
    }
}
