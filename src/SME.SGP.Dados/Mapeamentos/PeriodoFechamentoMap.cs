using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoFechamentoMap : BaseMap<PeriodoFechamento>
    {
        public PeriodoFechamentoMap()
        {
            ToTable("periodo_fechamento");
            Map(c => c.FechamentosBimestre).Ignore();
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Aplicacao).ToColumn("aplicacao");
        }
    }
}