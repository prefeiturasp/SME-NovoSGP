using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEEObservacaoMap : BaseMap<PlanoAEEObservacao>
    {
        public PlanoAEEObservacaoMap()
        {
            ToTable("plano_aee_observacao");
            Map(a => a.PlanoAEEId).ToColumn("plano_aee_id");
        }
    }
}
