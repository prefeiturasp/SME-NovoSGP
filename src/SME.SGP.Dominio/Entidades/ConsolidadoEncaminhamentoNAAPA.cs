using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Dominio
{
    public class ConsolidadoEncaminhamentoNAAPA : EntidadeBase
    {
        public ConsolidadoEncaminhamentoNAAPA()
        {
            
        }
        public ConsolidadoEncaminhamentoNAAPA(int anoLetivo, long ueId, long quantidade, SituacaoNAAPA situacao)
        {
            AnoLetivo = anoLetivo;
            UeId = ueId;
            Quantidade = quantidade;
            Situacao = situacao;
        }

        public int AnoLetivo { get; set; }
        public long UeId { get; set; }
        public long Quantidade { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
    }
}