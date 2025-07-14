using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ItineranciaQuestao : EntidadeBase
    {        
        public long QuestaoId { get; set; }
        public long? ArquivoId { get; set; }
        public string Resposta { get; set; }
        public bool Excluido { get; set; }
        public long ItineranciaId { get; set; }
    }
}