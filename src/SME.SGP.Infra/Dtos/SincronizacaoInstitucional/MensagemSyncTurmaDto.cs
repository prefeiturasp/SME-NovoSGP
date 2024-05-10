namespace SME.SGP.Infra
{
    public class MensagemSyncTurmaDto
    {
        public string UeId { get; set; }
        public long CodigoTurma { get; set; }

        public MensagemSyncTurmaDto() { }

        public MensagemSyncTurmaDto(string ueId, long codigoTurma)
        {
            this.UeId = ueId;
            this.CodigoTurma = codigoTurma;
        }
    }
}