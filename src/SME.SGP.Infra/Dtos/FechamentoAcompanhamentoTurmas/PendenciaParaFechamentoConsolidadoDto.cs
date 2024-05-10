using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class PendenciaParaFechamentoConsolidadoDto
    {
        public long PendenciaId { get; set; }
        public string Descricao { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }
}
