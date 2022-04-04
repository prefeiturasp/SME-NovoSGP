namespace SME.SGP.Infra.Dtos
{
    public class FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto
    {
        public FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; }
    }
}
