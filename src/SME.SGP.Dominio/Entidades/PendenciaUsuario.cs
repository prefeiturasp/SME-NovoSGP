using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaUsuario : EntidadeBase
    {
        public long PendenciaId { get; set; }
        [Computed]
        public Pendencia Pendencia { get; set; }
        public long UsuarioId { get; set; }
        [Computed]
        public Usuario Usuario { get; set; }
    }
}
