using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OpcaoQuestaoComplementarMap : BaseEntityMap<OpcaoQuestaoComplementar>
    {
        public OpcaoQuestaoComplementarMap()
        {
            ToTable("opcao_questao_complementar");
            Map(nameof(OpcaoQuestaoComplementar.OpcaoRespostaId), "opcao_resposta_id");
            Map(nameof(OpcaoQuestaoComplementar.QuestaoComplementarId), "questao_complementar_id");
        }
    }
}