using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PendenciaUsuario : EntidadeBase
    {
        public long PendenciaId { get; set; }
        public Pendencia Pendencia { get; set; }
        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
