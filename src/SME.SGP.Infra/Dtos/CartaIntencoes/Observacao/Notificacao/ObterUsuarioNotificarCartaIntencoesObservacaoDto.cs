namespace SME.SGP.Infra.Dtos
{
    public class ObterUsuarioNotificarCartaIntencoesObservacaoDto
    {
        public long TurmaId { get; set; }
        public long? ObservacaoId { get; set; }
        public long CartaIntencoesObservacaoId { get; set; }
    }
}
