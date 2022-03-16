using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEERespostaMap : BaseMap<PlanoAEEResposta>
    {
        public PlanoAEERespostaMap()
        {
            ToTable("plano_aee_resposta");
            Map(c => c.PlanoAEEQuestaoId).ToColumn("plano_questao_id");
            Map(c => c.RespostaId).ToColumn("resposta_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.Texto).ToColumn("texto");
            Map(c => c.PeriodoInicio).ToColumn("periodo_inicio");
            Map(c => c.PeriodoFim).ToColumn("periodo_fim");
        }
    }
}
