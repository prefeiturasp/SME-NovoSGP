using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class NotificacaoCartaIntencoesObservacao
    {
        public long Id { get; set; }
        public long NotificacaoId { get; set; }
        public long CartaIntencoesObservacaoId { get; set; }

    }
}
