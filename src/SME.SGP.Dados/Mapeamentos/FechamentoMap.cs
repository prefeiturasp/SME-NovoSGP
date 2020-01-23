using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoMap : BaseMap<Fechamento>
    {
        public FechamentoMap()
        {
            ToTable("fechamento");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.FechamentosBimestre).Ignore();
        }
    }
}