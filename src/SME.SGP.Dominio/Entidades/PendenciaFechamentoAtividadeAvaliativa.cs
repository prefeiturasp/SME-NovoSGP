using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PendenciaFechamentoAtividadeAvaliativa
    {
        public long Id { get; set; }
        public long AtividadeAvaliativaId { get; set; }
        public long PendenciaFechamentoId { get; set; }
    }
}
