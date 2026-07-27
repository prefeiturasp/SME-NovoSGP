using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ItineranciaAlunoQuestaoMap : BaseEntityMap<ItineranciaAlunoQuestao>
    {
        public ItineranciaAlunoQuestaoMap()
        {
            ToTable("itinerancia_aluno_questao");
            Map(nameof(ItineranciaAlunoQuestao.QuestaoId), "questao_id");
            Map(nameof(ItineranciaAlunoQuestao.Resposta), "resposta");
            Map(nameof(ItineranciaAlunoQuestao.Excluido), "excluido");
            Map(nameof(ItineranciaAlunoQuestao.ItineranciaAlunoId), "itinerancia_aluno_id");
        }
    }
}