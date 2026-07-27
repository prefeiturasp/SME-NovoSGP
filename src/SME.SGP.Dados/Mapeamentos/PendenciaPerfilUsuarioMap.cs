using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PendenciaPerfilUsuarioMap : BaseEntityMap<PendenciaPerfilUsuario>
    {
        public PendenciaPerfilUsuarioMap()
        {
            ToTable("pendencia_perfil_usuario");
            Map(nameof(PendenciaPerfilUsuario.PendenciaPerfilId), "pendencia_perfil_id");
            Map(nameof(PendenciaPerfilUsuario.UsuarioId), "usuario_id");
            Map(nameof(PendenciaPerfilUsuario.PerfilCodigo), "perfil_codigo");
        }
    }
}