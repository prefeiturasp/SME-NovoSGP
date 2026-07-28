using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEEReestruturacaoMap : BaseEntityMap<PlanoAEEReestruturacao>
    {
        public PlanoAEEReestruturacaoMap()
        {
            ToTable("plano_aee_reestruturacao");
            Map(nameof(PlanoAEEReestruturacao.PlanoAEEVersaoId), "plano_aee_versao_id");
            Map(nameof(PlanoAEEReestruturacao.Semestre), "semestre");
            Map(nameof(PlanoAEEReestruturacao.Descricao), "descricao");
        }
    }
}