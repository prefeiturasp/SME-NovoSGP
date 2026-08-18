using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class PendenciaPerfilUsuarioMap : BaseMap<PendenciaPerfilUsuario>
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