using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaEncaminhamentoAEEMap : BaseMap<PendenciaEncaminhamentoAEE>
    {
        public PendenciaEncaminhamentoAEEMap()
        {
            ToTable("pendencia_encaminhamento_aee");
            Map(c => c.EncaminhamentoAEEId).ToColumn("encaminhamento_aee_id");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
        }
    }
}
