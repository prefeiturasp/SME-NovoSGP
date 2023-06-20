namespace SME.SGP.Infra
{
    public class MensagemConsolidarTurmaConselhoClasseAlunoPorDreAnoDto
    {
        public MensagemConsolidarTurmaConselhoClasseAlunoPorDreAnoDto(int anoLetivo, long dreId = 0)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
        }

        public int AnoLetivo { get; set;  }
        public long DreId { get; set; }
    }
}
