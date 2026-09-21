using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class PlanoAEEObservacaoMap : BaseMap<PlanoAEEObservacao>
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