using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ItineranciaAlunoQuestao : EntidadeBase
    {        
        public long QuestaoId { get; set; }
        public string Resposta { get; set; }
        public bool Excluido { get; set; }
        public long ItineranciaAlunoId { get; set; }
    }
}