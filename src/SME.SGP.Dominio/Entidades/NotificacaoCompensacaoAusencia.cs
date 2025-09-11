using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class NotificacaoCompensacaoAusencia
    {
        public long Id { get; set; }
        public long NotificacaoId { get; set; }
        public long CompensacaoAusenciaId { get; set; }
    }
}
