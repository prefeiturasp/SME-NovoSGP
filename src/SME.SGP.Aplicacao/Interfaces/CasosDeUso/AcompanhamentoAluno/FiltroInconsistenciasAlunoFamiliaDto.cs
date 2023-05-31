namespace SME.SGP.Aplicacao.Interfaces
{
    public class FiltroInconsistenciasAlunoFamiliaDto
    {
        public FiltroInconsistenciasAlunoFamiliaDto()
        {
            
        }
        public FiltroInconsistenciasAlunoFamiliaDto(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public  long TurmaId { get; set; }
        public  int Bimestre { get; set; }
    }
}