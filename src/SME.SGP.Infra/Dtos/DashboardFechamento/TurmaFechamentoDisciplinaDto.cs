namespace SME.SGP.Infra.Dtos
{
    public class TurmaFechamentoDisciplinaDto
    {
        public long TurmaId { get; set; }
        public int QuantidadeDisciplinas { get; set; }
        public long PeriodoEscolarId { get; set; }
        public long DisciplinaId { get; set; }
        public int Situacao { get; set; }
    }
}