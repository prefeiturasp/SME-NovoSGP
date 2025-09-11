using SME.SGP.Dominio.Enumerados;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConsolidadoEncaminhamentoNAAPA : EntidadeBase
    {
        public ConsolidadoEncaminhamentoNAAPA()
        {
            
        }
        public ConsolidadoEncaminhamentoNAAPA(int anoLetivo, long ueId, long quantidade, SituacaoNAAPA situacao, Modalidade modalidade)
        {
            AnoLetivo = anoLetivo;
            UeId = ueId;
            Quantidade = quantidade;
            Situacao = situacao;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public long UeId { get; set; }
        public long Quantidade { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
        public Modalidade Modalidade { get; set; }
    }
}