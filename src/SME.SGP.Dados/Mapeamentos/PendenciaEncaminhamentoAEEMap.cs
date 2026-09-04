using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaEncaminhamentoAEEMap : BaseMap<PendenciaEncaminhamentoAEE>
    {
        public PendenciaEncaminhamentoAEEMap()
        {
            ToTable("pendencia_encaminhamento_aee");
            Map(nameof(PendenciaEncaminhamentoAEE.EncaminhamentoAEEId), "encaminhamento_aee_id");
            Map(nameof(PendenciaEncaminhamentoAEE.PendenciaId), "pendencia_id");
        }
    }
}