using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OpcaoQuestaoComplementarMap : BaseMap<OpcaoQuestaoComplementar>
    {
        public OpcaoQuestaoComplementarMap()
        {
            ToTable("opcao_questao_complementar");
            Map(c => c.OpcaoRespostaId).ToColumn("opcao_resposta_id");
            Map(c => c.QuestaoComplementarId).ToColumn("questao_complementar_id");
        }
    }
}
