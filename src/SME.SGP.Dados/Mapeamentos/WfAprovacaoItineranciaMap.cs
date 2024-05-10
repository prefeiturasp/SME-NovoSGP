using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WfAprovacaoItineranciaMap : DommelEntityMap<WfAprovacaoItinerancia>
    {
        public WfAprovacaoItineranciaMap()
        {
            ToTable("wf_aprovacao_itinerancia");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.WfAprovacaoId).ToColumn("wf_aprovacao_id");
            Map(c => c.ItineranciaId).ToColumn("itinerancia_id");
            Map(c => c.StatusAprovacao).ToColumn("status_aprovacao");
        }
    }
}
