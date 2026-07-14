namespace SME.SGP.Infra.Dtos
{
    public class ExcluirNotificacaoDiarioBordoDto
    {
        public ExcluirNotificacaoDiarioBordoDto(long observacaoId)
        {
            ObservacaoId = observacaoId;
        }

        public long ObservacaoId { get; set; }
    }
}
