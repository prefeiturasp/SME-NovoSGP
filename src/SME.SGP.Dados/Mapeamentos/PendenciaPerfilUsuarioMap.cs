using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PendenciaPerfilUsuarioMap : BaseMap<PendenciaPerfilUsuario>
    {
        public PendenciaPerfilUsuarioMap()
        {
            ToTable("pendencia_perfil_usuario");
            Map(c => c.PendenciaPerfilId).ToColumn("pendencia_perfil_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.PerfilCodigo).ToColumn("perfil_codigo");
        }
    }
}
