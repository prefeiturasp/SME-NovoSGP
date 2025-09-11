using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ComunicadoAnoEscolar
    {
        public ComunicadoAnoEscolar()
        {
        }
        public long ComunicadoId { get; set; }
        public string AnoEscolar { get; set; }
        public long Id { get; set; }
    }
}
