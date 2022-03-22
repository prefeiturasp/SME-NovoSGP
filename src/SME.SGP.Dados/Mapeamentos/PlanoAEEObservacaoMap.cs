using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEEObservacaoMap : BaseMap<PlanoAEEObservacao>
    {
        public PlanoAEEObservacaoMap()
        {
            ToTable("plano_aee_observacao");
            Map(c => c.PlanoAEEId).ToColumn("plano_aee_id");
            Map(c => c.Observacao).ToColumn("observacao");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
