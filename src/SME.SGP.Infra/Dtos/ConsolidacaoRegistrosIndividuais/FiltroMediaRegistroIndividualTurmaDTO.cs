namespace SME.SGP.Infra
{
    public class FiltroMediaRegistroIndividualTurmaDTO
    {
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }

        public FiltroMediaRegistroIndividualTurmaDTO(long turmaId, int anoLetivo)
        {
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
        }
    }
}
