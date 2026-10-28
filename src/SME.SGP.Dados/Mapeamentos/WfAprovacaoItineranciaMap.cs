using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WfAprovacaoItineranciaMap : SimpleMap<WfAprovacaoItinerancia>
    {
        public WfAprovacaoItineranciaMap()
        {
            ToTable("wf_aprovacao_itinerancia");
            Map(nameof(WfAprovacaoItinerancia.WfAprovacaoId), "wf_aprovacao_id");
            Map(nameof(WfAprovacaoItinerancia.ItineranciaId), "itinerancia_id");
            Map(nameof(WfAprovacaoItinerancia.StatusAprovacao), "status_aprovacao");
        }
    }
}