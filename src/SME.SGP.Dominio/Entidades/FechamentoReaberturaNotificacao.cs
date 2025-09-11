using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class FechamentoReaberturaNotificacao
    {
        public long FechamentoReaberturaId { get; set; }
        public long Id { get; set; }
        public long NotificacaoId { get; set; }
    }
}