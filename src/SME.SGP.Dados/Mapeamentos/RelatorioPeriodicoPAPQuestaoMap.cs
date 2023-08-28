using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioPeriodicoPAPQuestaoMap : BaseMap<RelatorioPeriodicoPAPQuestao>
    {
        public RelatorioPeriodicoPAPQuestaoMap()
        {
            ToTable("relatorio_periodico_pap_questao");

            Map(c => c.RelatorioPeriodiocoSecaoId).ToColumn("relatorio_periodico_pap_secao_id");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
