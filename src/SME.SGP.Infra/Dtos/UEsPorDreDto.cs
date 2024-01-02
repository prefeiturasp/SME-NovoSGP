using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class UEsPorDreDto
    {
        public string CodigoDre { get; set; } 
        public Modalidade? Modalidade { get; set; }
        public int Periodo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public bool ConsideraNovasUEs { get; set; }
        public bool FiltrarTipoEscolaPorAnoLetivo { get; set; }
        public string Filtro { get; set; }
    }
}
