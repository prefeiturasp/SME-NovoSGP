namespace SME.SGP.Infra
{
    public class FiltroJustificativasAlunoPorComponenteCurricular
    {
        public FiltroJustificativasAlunoPorComponenteCurricular(long turmaId, long componenteCurricularCodigo, long alunoCodigo, int bimestre)
        {
            TurmaId = turmaId;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public long AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
    }
}
