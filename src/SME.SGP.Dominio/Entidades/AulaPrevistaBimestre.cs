using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class AulaPrevistaBimestre : EntidadeBase
    {
        public long AulaPrevistaId { get; set; }

        public AulaPrevista AulaPrevista { get; set; }

        public int Previstas { get; set; }

        public int Bimestre { get; set; }
    }
}
