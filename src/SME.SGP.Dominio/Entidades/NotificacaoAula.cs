using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class NotificacaoAula
    {
        public long Id { get; set; }
        public long NotificacaoId { get; set; }
        public long AulaId { get; set; }
    }
}
