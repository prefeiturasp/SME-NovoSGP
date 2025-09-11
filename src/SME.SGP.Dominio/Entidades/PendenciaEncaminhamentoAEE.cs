using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PendenciaEncaminhamentoAEE : EntidadeBase
    {
        public long EncaminhamentoAEEId { get; set; }
        public long PendenciaId { get; set; }
        public Pendencia Pendencia { get; set; }
        public EncaminhamentoAEE EncaminhamentoAEE { get; set; }
    }
}
