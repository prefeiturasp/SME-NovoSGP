using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PendenciaFechamentoAula
    {
        public long Id { get; set; }
        public long AulaId { get; set; }
        public long PendenciaFechamentoId { get; set; }
    }
}
