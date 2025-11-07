using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class TurmaModalidadeSerieAnoDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string SerieAno { get; set; }
        public int AnoLetivo { get; set; }
    }
}
