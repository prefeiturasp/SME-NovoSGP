using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PendenciaPerfilMap : BaseEntityMap<PendenciaPerfil>
    {
        public PendenciaPerfilMap()
        {
            ToTable("pendencia_perfil");
            Map(nameof(PendenciaPerfil.PerfilCodigo), "perfil_codigo");
            Map(nameof(PendenciaPerfil.PendenciaId), "pendencia_id");
        }
    }
}