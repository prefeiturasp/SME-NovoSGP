namespace SME.SGP.Infra.Dtos
{
    public class SalvarNotificacaoDevolutivaDto
    {
        public SalvarNotificacaoDevolutivaDto(long turmaId, string usuarioNome, string usuarioRF, long devolutivaId)
        {
            TurmaId = turmaId;
            UsuarioNome = usuarioNome;
            UsuarioRF = usuarioRF;
            DevolutivaId = devolutivaId;
        }

        public long TurmaId { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRF { get; set; }
        public long DevolutivaId { get; set; }
    }
}
