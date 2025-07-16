using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ComunicadoTipoEscola
    {
        public ComunicadoTipoEscola()
        {
        }
        public long ComunicadoId { get; set; }
        public long TipoEscola { get; set; }
        public long Id { get; set; }
    }
}
