using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ItineranciaObjetivo : EntidadeBase
    {        
        public long ItineranciaObjetivosBaseId { get; set; }
        public long ItineranciaId { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
    }
}