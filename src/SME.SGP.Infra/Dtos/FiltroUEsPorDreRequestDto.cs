using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class FiltroUEsPorDreRequestDto
    {
        public Modalidade? Modalidade { get; set; }
        public int Periodo { get; set; } = 0;
        public int AnoLetivo { get; set; } = 0;
        public bool ConsideraNovasUEs { get; set; } = false;
        public bool FiltrarTipoEscolaPorAnoLetivo { get; set; } = false;
        public string Filtro { get; set; } = "";
        public string[] AnosTurma { get; set; }
    }
}
