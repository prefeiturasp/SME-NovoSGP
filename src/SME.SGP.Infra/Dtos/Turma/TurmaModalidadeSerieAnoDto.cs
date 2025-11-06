using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class TurmaModalidadeSerieAnoDto
    {
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int SeriAno { get; set; }
        public int AnoLetivo { get; set; }
    }
}
