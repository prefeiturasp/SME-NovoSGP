namespace SME.SGP.Infra.Dtos
{
    public class TurmaBimestreDto
    {
        public TurmaBimestreDto()
        { }
        public TurmaBimestreDto(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }
}
