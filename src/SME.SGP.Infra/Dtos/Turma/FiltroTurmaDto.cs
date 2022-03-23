using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroTurmaDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade? Modalidade { get; set; }
        public string TurmaCodigo { get; set; }
        public int Bimestre { get; set; }
        public int? Semestre { get; set; }
        public bool ConsideraHistorico { get; set; } = false;
    }
}
