using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class HistoricoNotaFechamento
    {
        public long HistoricoNotaId { get; set; }
        public long Id {get; set;}
        public long FechamentoNotaId { get; set; }
        public long? WorkFlowId { get; set; }
    }
}
