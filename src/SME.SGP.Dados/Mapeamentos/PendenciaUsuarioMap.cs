using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PendenciaUsuarioMap : BaseEntityMap<PendenciaUsuario>
    {
        public PendenciaUsuarioMap()
        {
            ToTable("pendencia_usuario");
            Map(nameof(PendenciaUsuario.PendenciaId), "pendencia_id");
            Map(nameof(PendenciaUsuario.UsuarioId), "usuario_id");
        }
    }
}