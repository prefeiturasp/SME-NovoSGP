using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class PendenciaPerfilMap : BaseMap<PendenciaPerfil>
    {
        public PendenciaPerfilMap()
        {
            ToTable("pendencia_perfil");
            Map(nameof(PendenciaPerfil.PerfilCodigo), "perfil_codigo");
            Map(nameof(PendenciaPerfil.PendenciaId), "pendencia_id");
        }
    }
}