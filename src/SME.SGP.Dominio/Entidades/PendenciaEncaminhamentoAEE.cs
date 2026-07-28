using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaEncaminhamentoAEE : EntidadeBase
    {
        public long EncaminhamentoAEEId { get; set; }
        public long PendenciaId { get; set; }
        [Computed]
        public Pendencia Pendencia { get; set; }
        [Computed]
        public EncaminhamentoAEE EncaminhamentoAEE { get; set; }
    }
}
