using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio.Entidades
{
    public class OpcaoQuestaoComplementar : EntidadeBase
    {
        public long OpcaoRespostaId { get; set; }
        [Computed]
        public OpcaoResposta OpcaoResposta { get; set; }
        [Computed]
        public Questao QuestaoComplementar { get; set; }
        public long QuestaoComplementarId { get; set; }
    }
}
