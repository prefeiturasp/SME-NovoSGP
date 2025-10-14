namespace SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAbandono
{
    public class ConsolidacaoAbandonoUeDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string CodigoTurma { get; set; }
        public string NomeTurma { get; set; }
        public string Modalidade { get; set; }
        public int QuantidadeDesistencias { get; set; }
        public int AnoLetivo { get; set; }
    }
}
