namespace SME.SGP.Infra
{
    public class FiltroJustificativasAlunoPorComponenteCurricular
    {
        public FiltroJustificativasAlunoPorComponenteCurricular(long turmaId, long componenteCurricularCodigo, long alunoCodigo)
        {
            TurmaId = turmaId;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public long AlunoCodigo { get; set; }
    }
}
