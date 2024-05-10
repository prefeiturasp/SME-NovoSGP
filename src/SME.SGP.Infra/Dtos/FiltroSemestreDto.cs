using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroSemestreDto
    {
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int AnoLetivo { get; set; } = 0;
        public Modalidade Modalidade { get; set; }
    }
}