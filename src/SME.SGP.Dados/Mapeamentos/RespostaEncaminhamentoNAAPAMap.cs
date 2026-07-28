using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class RespostaEncaminhamentoNAAPAMap : BaseEntityMap<RespostaEncaminhamentoNAAPA>
    {
        public RespostaEncaminhamentoNAAPAMap()
        {
            ToTable("encaminhamento_naapa_resposta");

            Map(nameof(RespostaEncaminhamentoNAAPA.QuestaoEncaminhamentoId), "questao_encaminhamento_id");
            Map(nameof(RespostaEncaminhamentoNAAPA.RespostaId), "resposta_id");
            Map(nameof(RespostaEncaminhamentoNAAPA.ArquivoId), "arquivo_id");
            Map(nameof(RespostaEncaminhamentoNAAPA.Texto), "texto");
            Map(nameof(RespostaEncaminhamentoNAAPA.Excluido), "excluido");
        }
    }
}