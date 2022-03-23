namespace SME.SGP.Infra
{
    public class FiltroTratamentoAtribuicaoPendenciaDto
    {
        public FiltroTratamentoAtribuicaoPendenciaDto(long pendenciaId, long ueId)
        {
            PendenciaId = pendenciaId;
            UeId = ueId;
        }

        public long PendenciaId { get; set; }
        public long UeId { get; set; }
    }
}
