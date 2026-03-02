namespace SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao
{
    public class ConsolidacaoAprovacaoDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public long TurmaId { get; set; }
        public string Turma { get; set; }
        public string SerieAno { get; set; }
        public int CodigoModalidade { get; set; }
        public string Modalidade { get; set; }
        public int ParecerConclusivoId { get; set; }
        public string ParecerDescricao { get; set; }
        public int AnoLetivo { get; set; }
    }
}
