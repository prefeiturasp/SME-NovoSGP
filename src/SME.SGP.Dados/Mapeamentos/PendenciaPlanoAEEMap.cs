using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class PendenciaPlanoAEEMap : BaseMap<PendenciaPlanoAEE>
    {
        public PendenciaPlanoAEEMap()
        {
            ToTable("pendencia_plano_aee");
            Map(nameof(PendenciaPlanoAEE.PlanoAEEId), "plano_aee_id");
            Map(nameof(PendenciaPlanoAEE.PendenciaId), "pendencia_id");
        }
    }
}