namespace SME.SGP.Infra
{
    public class AlterarNotificacaoDiarioBordoDto
    {
        public AlterarNotificacaoDiarioBordoDto(long observacaoId, long usuarioId)
        {
            ObservacaoId = observacaoId;
            UsuarioId = usuarioId;
        }

        public long ObservacaoId { get; set; }

        public long UsuarioId { get; set; }
    }
}
