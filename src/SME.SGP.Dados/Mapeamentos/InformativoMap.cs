using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformativoMap : BaseMap<Informativo>
    {
        public InformativoMap()
        {
            ToTable("informativo");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Titulo).ToColumn("titulo");
            Map(c => c.Texto).ToColumn("texto");
        }
    }
}
