using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoFechamentoMap : BaseMap<PeriodoFechamento>
    {
        public PeriodoFechamentoMap()
        {
            ToTable("periodo_fechamento");
            Map(nameof(PeriodoFechamento.DreId), "dre_id");
            Map(nameof(PeriodoFechamento.Migrado), "migrado");
            Map(nameof(PeriodoFechamento.UeId), "ue_id");
            Map(nameof(PeriodoFechamento.Aplicacao), "aplicacao");
        }
    }
}