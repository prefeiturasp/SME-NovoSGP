using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroRelatorioParecerConclusivoDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade? Modalidade { get; set; }
        public int? Semestre { get; set; }
        public long Ciclo { get; set; }
        public long ParecerConclusivoId { get; set; }
        public string[] Anos { get; set; }
        public string UsuarioNome { get; set; }
        public TipoFormatoRelatorio TipoFormatoRelatorio { get; set; }
    }
}