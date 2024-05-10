namespace SME.SGP.Infra
{
    public class FiltroMigracaoFrequenciaTurmaDto
    {
        public FiltroMigracaoFrequenciaTurmaDto() { }
        public FiltroMigracaoFrequenciaTurmaDto(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }
}
