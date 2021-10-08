using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaAlunoQuestaoMap : BaseMap<ItineranciaAlunoQuestao>
    {
        public ItineranciaAlunoQuestaoMap()
        {
            ToTable("itinerancia_aluno_questao");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Resposta).ToColumn("resposta");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.ItineranciaAlunoId).ToColumn("itinerancia_aluno_id");
        }
    }
}
