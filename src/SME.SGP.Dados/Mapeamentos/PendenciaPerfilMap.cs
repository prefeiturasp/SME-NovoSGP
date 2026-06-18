using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PendenciaPerfilMap : BaseMap<PendenciaPerfil>
    {
        public PendenciaPerfilMap()
        {
            ToTable("pendencia_perfil");
            Map(c => c.PerfilCodigo).ToColumn("perfil_codigo");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
        }
    }
}
