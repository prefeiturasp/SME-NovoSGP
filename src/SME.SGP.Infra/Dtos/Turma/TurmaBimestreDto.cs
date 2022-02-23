namespace SME.SGP.Infra.Dtos
{
    public class TurmaBimestreDto
    {
        public TurmaBimestreDto(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; }
        public int Bimestre { get; }
    }
}
