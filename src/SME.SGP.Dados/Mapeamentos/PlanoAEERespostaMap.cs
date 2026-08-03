using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class PlanoAEERespostaMap : BaseMap<PlanoAEEResposta>
    {
        public PlanoAEERespostaMap()
        {
            ToTable("plano_aee_resposta");
            Map(nameof(PlanoAEEResposta.PlanoAEEQuestaoId), "plano_questao_id");
            Map(nameof(PlanoAEEResposta.RespostaId), "resposta_id");
            Map(nameof(PlanoAEEResposta.ArquivoId), "arquivo_id");
            Map(nameof(PlanoAEEResposta.Texto), "texto");
            Map(nameof(PlanoAEEResposta.PeriodoInicio), "periodo_inicio");
            Map(nameof(PlanoAEEResposta.PeriodoFim), "periodo_fim");
        }
    }
}