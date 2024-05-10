using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DetalhamentoPendenciaAulaDto
    {
        public long PendenciaId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
        public string DescricaoHtml { get; set; }
    }
}
