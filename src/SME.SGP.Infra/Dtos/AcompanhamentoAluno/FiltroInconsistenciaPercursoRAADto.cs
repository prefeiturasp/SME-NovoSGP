namespace SME.SGP.Infra
{
    public class FiltroInconsistenciaPercursoRAADto
    {
        public FiltroInconsistenciaPercursoRAADto(long turmaId, int semestre)
        {
            TurmaId = turmaId;
            Semestre = semestre;
        }

        public long TurmaId { get; set; } 
        public int Semestre {  get; set; }
    }
}
