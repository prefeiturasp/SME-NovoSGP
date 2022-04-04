namespace SME.SGP.Infra
{
    public class MensagemConsolidarTurmaConselhoClasseAlunoPorTurmaDto
    {
        public long TurmaId { get; set; }

        public MensagemConsolidarTurmaConselhoClasseAlunoPorTurmaDto(long turmaId)
        {
            this.TurmaId = turmaId;
        }
    }
}
