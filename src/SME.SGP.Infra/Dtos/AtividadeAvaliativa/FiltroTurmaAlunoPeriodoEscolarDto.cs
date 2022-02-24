namespace SME.SGP.Infra
{
    public class FiltroTurmaAlunoPeriodoEscolarDto
    {
        public FiltroTurmaAlunoPeriodoEscolarDto(long turmaId, long periodoEscolarId, string alunoCodigo, string componenteCurricular)
        {
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
            AlunoCodigo = alunoCodigo;
            ComponenteCurricular = componenteCurricular;
        }

        public long TurmaId { get; }
        public long PeriodoEscolarId { get; }
        public string AlunoCodigo { get; }
        public string ComponenteCurricular { get; }
    }
}
