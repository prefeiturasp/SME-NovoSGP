using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioAtribuicaoCJDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade? Modalidade { get; set; }
        public int? Semestre { get; set; }
        public string TurmaCodigo { get; set; }
        public string UsuarioRf { get; set; }
        public bool ExibirAulas { get; set; }
        public bool ExibirAtribuicoesExporadicas { get; set; }
        public TipoVisualizacao TipoVisualizacao { get; set; }
    }
}
