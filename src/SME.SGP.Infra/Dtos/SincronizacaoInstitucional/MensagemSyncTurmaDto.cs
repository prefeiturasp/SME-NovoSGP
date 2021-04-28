namespace SME.SGP.Infra
{
    public class MensagemSyncTurmaDto
    {
        public string UeId;
        public long CodigoTurma;

        public MensagemSyncTurmaDto(string ueId, long codigoTurma)
        {
            this.UeId = ueId;
            this.CodigoTurma = codigoTurma;
        }
    }
}