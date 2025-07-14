using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio.Entidades
{
    [ExcludeFromCodeCoverage]
    public class OpcaoQuestaoComplementar : EntidadeBase
    {
        public long OpcaoRespostaId { get; set; }
        public OpcaoResposta OpcaoResposta { get; set; }
        public Questao QuestaoComplementar { get; set; }
        public long QuestaoComplementarId { get; set; }
    }
}
