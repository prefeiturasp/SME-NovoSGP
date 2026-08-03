using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class RespostaEncaminhamentoAEEMap : BaseMap<RespostaEncaminhamentoAEE>
    {
        public RespostaEncaminhamentoAEEMap()
        {
            ToTable("resposta_encaminhamento_aee");

            Map(nameof(RespostaEncaminhamentoAEE.QuestaoEncaminhamentoId), "questao_encaminhamento_id");
            Map(nameof(RespostaEncaminhamentoAEE.RespostaId), "resposta_id");
            Map(nameof(RespostaEncaminhamentoAEE.ArquivoId), "arquivo_id");
            Map(nameof(RespostaEncaminhamentoAEE.Texto), "texto");
            Map(nameof(RespostaEncaminhamentoAEE.Excluido), "excluido");
        }
    }
}