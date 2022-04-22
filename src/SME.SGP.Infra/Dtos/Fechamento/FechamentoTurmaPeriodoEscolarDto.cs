namespace SME.SGP.Infra.Dtos
{
    public class FechamentoTurmaPeriodoEscolarDto
    {
        public long FechamentoTurmaId { get; set; }
        public long? PeriodoEscolarId { get; set; }
        public bool PossuiAvaliacao { get; set; }
    }
}
