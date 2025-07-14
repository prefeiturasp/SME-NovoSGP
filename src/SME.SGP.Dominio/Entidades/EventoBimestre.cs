using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio.Entidades
{
    [ExcludeFromCodeCoverage]
    public class EventoBimestre : EntidadeBase
    {
        public long EventoId { get; set; }
        public int? Bimestre { get; set; }
    }
}
