namespace SME.SGP.Infra.Dtos
{
    public class FiltroNotificacaoCompensacaoAusenciaDto 
    {
        public FiltroNotificacaoCompensacaoAusenciaDto(long compensacaoId)
        {
            CompensacaoId = compensacaoId;
        }

        public long CompensacaoId { get; set; }
    }
}
