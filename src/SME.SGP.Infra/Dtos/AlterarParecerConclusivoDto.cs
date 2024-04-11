namespace SME.SGP.Infra
{
    public class AlterarParecerConclusivoDto
    {
        public long ConselhoClasseId { get; set; } 
        public long FechamentoTurmaId { get; set; } 
        public string AlunoCodigo { get; set; } 
        public long? ParecerConclusivoId { get; set; }
    }
}
