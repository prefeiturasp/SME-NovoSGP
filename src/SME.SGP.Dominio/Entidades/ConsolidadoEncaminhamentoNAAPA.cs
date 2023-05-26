using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Dominio
{
    public class ConsolidadoEncaminhamentoNAAPA : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public long UeId { get; set; }
        public long Quantidade { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
    }
}