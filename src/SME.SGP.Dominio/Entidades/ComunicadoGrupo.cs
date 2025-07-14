using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ComunicadoGrupo
    {
        public long ComunicadoId { get; set; }
        public long GrupoComunicadoId { get; set; }
        public long Id { get; set; }
    }
}