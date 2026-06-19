using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PendenciaUsuarioMap : BaseMap<PendenciaUsuario>
    {
        public PendenciaUsuarioMap()
        {
            ToTable("pendencia_usuario");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
        }
    }
}
