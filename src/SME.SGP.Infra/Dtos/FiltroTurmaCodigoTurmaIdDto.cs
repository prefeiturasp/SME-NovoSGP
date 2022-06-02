namespace SME.SGP.Infra
{
    public class FiltroTurmaCodigoTurmaIdDto
    {
        public FiltroTurmaCodigoTurmaIdDto() { }
        public FiltroTurmaCodigoTurmaIdDto(string turmaCodigo, long turmaId)
        {
            TurmaCodigo = turmaCodigo;
            TurmaId = turmaId;
        }

        public string TurmaCodigo { get; set; }

        public long TurmaId { get; set; }
    }
}