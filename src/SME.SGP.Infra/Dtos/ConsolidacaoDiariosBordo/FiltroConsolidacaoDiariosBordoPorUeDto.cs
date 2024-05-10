namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoDiariosBordoPorUeDto
    {
        public FiltroConsolidacaoDiariosBordoPorUeDto(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set;  }
    }
}
