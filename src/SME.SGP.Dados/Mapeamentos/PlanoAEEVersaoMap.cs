using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEEVersaoMap : BaseMap<PlanoAEEVersao>
    {
        public PlanoAEEVersaoMap()
        {
            ToTable("plano_aee_versao");
            Map(c => c.PlanoAEEId).ToColumn("plano_aee_id");
            Map(c => c.Numero).ToColumn("numero");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
