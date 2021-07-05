using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ObterPeriodosParaConsultaNotasFiltroDto
    {
        public Modalidade Modalidade { get; set; }
        public int AnoLetivo { get; set; }
    }
}
