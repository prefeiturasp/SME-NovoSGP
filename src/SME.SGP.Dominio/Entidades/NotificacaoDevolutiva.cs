using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class NotificacaoDevolutiva
    {
        public long Id { get; set; }
        public long DevolutivaId { get; set; }
        public long NotificacaoId { get; set; }
    }
}
