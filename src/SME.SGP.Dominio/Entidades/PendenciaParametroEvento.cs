using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PendenciaParametroEvento : EntidadeBase
    {
        public long PendenciaCalendarioUeId { get; set; }
        public PendenciaCalendarioUe PendenciaCalendarioUe { get; set; }
        public long ParametroSistemaId { get; set; }
        public int QuantidadeEventos { get; set; }
    }
}
