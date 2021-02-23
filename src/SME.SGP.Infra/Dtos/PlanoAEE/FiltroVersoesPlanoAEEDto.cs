namespace SME.SGP.Infra
{
    public class FiltroVersoesPlanoAEEDto
    {
        public FiltroVersoesPlanoAEEDto(long planoAEEId, long reestruturacaoId)
        {
            PlanoId = planoAEEId;
            ReestruturacaoId = reestruturacaoId;
        }

        public long PlanoId { get; set; }
        public long ReestruturacaoId { get; set; }
    }
}
