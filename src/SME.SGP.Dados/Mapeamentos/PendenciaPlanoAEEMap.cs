using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PendenciaPlanoAEEMap : BaseMap<PendenciaPlanoAEE>
    {
        public PendenciaPlanoAEEMap()
        {
            ToTable("pendencia_plano_aee");
            Map(c => c.PlanoAEEId).ToColumn("plano_aee_id");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
        }
    }
}
