namespace SME.SGP.Infra
{
    public class RetornoPlanoAEEDto {
        public long PlanoId { get; set; }
        public long PlanoVersaoId { get; set; }

        public RetornoPlanoAEEDto(long planoId, long planoVersaoId)
        {
            PlanoId = planoId;
            PlanoVersaoId = planoVersaoId;
        }
    }
        
}
