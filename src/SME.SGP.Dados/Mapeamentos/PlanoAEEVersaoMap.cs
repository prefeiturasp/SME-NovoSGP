using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEEVersaoMap : BaseEntityMap<PlanoAEEVersao>
    {
        public PlanoAEEVersaoMap()
        {
            ToTable("plano_aee_versao");
            Map(nameof(PlanoAEEVersao.PlanoAEEId), "plano_aee_id");
            Map(nameof(PlanoAEEVersao.Numero), "numero");
            Map(nameof(PlanoAEEVersao.Excluido), "excluido");
        }
    }
}