using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEEObservacaoMap : BaseEntityMap<PlanoAEEObservacao>
    {
        public PlanoAEEObservacaoMap()
        {
            ToTable("plano_aee_observacao");
            Map(nameof(PlanoAEEObservacao.PlanoAEEId), "plano_aee_id");
            Map(nameof(PlanoAEEObservacao.Observacao), "observacao");
            Map(nameof(PlanoAEEObservacao.Excluido), "excluido");
        }
    }
}