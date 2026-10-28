using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class PendenciaUsuarioMap : BaseMap<PendenciaUsuario>
    {
        public PendenciaUsuarioMap()
        {
            ToTable("pendencia_usuario");
            Map(nameof(PendenciaUsuario.PendenciaId), "pendencia_id");
            Map(nameof(PendenciaUsuario.UsuarioId), "usuario_id");
        }
    }
}