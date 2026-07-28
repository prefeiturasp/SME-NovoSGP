using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class AulaPrevistaBimestre : EntidadeBase
    {
        public long AulaPrevistaId { get; set; }

        [Computed]
        public AulaPrevista AulaPrevista { get; set; }

        public int Previstas { get; set; }

        public int Bimestre { get; set; }
    }
}
