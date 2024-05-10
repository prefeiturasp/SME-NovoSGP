namespace SME.SGP.Infra
{
    public class FiltroCompensacaoAusenciaDto
    {
        public FiltroCompensacaoAusenciaDto(long[] compensacaoAusenciaIds)
        {
            CompensacaoAusenciaIds = compensacaoAusenciaIds;
        }
        public long[] CompensacaoAusenciaIds { get; set; }
    }
}
