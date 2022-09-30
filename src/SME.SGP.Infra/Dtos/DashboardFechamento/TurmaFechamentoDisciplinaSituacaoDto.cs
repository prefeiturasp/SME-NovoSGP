namespace SME.SGP.Infra.Dtos
{
    public class TurmaFechamentoDisciplinaSituacaoDto
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public long DisciplinaId { get; set; }
        public int Situacao { get; set; }
    }
}