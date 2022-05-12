namespace SME.SGP.Infra
{
    public class FiltroTurmaAlunoPeriodoEscolarDto
    {
        public FiltroTurmaAlunoPeriodoEscolarDto(long turmaId, long periodoEscolarId, string alunoCodigo)
        {
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; }
        public long PeriodoEscolarId { get; }
        public string AlunoCodigo { get; }
    }
}
