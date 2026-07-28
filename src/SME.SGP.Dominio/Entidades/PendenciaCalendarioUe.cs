using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaCalendarioUe : EntidadeBase
    {
        public long PendenciaId { get; set; }
        [Computed]
        public Pendencia Pendencia { get; set; }
        public long UeId { get; set; }
        [Computed]
        public Ue Ue { get; set; }
        public long TipoCalendarioId { get; set; }
        [Computed]
        public TipoCalendario TipoCalendario { get; set; }
    }
}
