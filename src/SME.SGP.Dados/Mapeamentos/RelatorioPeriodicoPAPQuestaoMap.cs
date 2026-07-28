using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioPeriodicoPAPQuestaoMap : BaseEntityMap<RelatorioPeriodicoPAPQuestao>
    {
        public RelatorioPeriodicoPAPQuestaoMap()
        {
            ToTable("relatorio_periodico_pap_questao");

            Map(nameof(RelatorioPeriodicoPAPQuestao.RelatorioPeriodiocoSecaoId), "relatorio_periodico_pap_secao_id");
            Map(nameof(RelatorioPeriodicoPAPQuestao.QuestaoId), "questao_id");
            Map(nameof(RelatorioPeriodicoPAPQuestao.Excluido), "excluido");
        }
    }
}