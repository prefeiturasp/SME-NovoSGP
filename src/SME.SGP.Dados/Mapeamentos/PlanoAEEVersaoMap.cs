using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEEVersaoMap : BaseMap<PlanoAEEVersao>
    {
        public PlanoAEEVersaoMap()
        {
            ToTable("plano_aee_versao");
            Map(a => a.PlanoAEEId).ToColumn("plano_aee_id");
        }
    }
}
