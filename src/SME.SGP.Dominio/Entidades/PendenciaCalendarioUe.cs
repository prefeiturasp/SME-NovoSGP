using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PendenciaCalendarioUe : EntidadeBase
    {
        public long PendenciaId { get; set; }
        public Pendencia Pendencia { get; set; }
        public long UeId { get; set; }
        public Ue Ue { get; set; }
        public long TipoCalendarioId { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
    }
}
