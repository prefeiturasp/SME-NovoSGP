using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioPeriodicoPAPRespostaMap : BaseMap<RelatorioPeriodicoPAPResposta>
    {
        public RelatorioPeriodicoPAPRespostaMap()
        {
            ToTable("relatorio_periodico_pap_resposta");

            Map(nameof(RelatorioPeriodicoPAPResposta.RelatorioPeriodicoQuestaoId), "relatorio_periodico_pap_questao_id");
            Map(nameof(RelatorioPeriodicoPAPResposta.RespostaId), "resposta_id");
            Map(nameof(RelatorioPeriodicoPAPResposta.ArquivoId), "arquivo_id");
            Map(nameof(RelatorioPeriodicoPAPResposta.Texto), "texto");
            Map(nameof(RelatorioPeriodicoPAPResposta.Excluido), "excluido");
        }
    }
}