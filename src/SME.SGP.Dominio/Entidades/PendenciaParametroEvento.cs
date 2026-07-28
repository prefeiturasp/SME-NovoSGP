using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaParametroEvento : EntidadeBase
    {
        public long PendenciaCalendarioUeId { get; set; }
        [Computed]
        public PendenciaCalendarioUe PendenciaCalendarioUe { get; set; }
        public long ParametroSistemaId { get; set; }
        public int QuantidadeEventos { get; set; }
    }
}
