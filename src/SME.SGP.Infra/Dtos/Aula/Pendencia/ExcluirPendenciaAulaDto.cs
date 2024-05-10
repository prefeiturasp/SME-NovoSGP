using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ExcluirPendenciaAulaDto
    {
        public long AulaId { get; set; }
        public TipoPendencia TipoPendenciaAula { get; set; }
    }
}
