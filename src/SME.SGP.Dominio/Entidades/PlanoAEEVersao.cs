using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PlanoAEEVersao : EntidadeBase
    {
        public long PlanoAEEId { get; set; }
        public PlanoAEE PlanoAEE { get; set; }

        public int Numero { get; set; }

        public bool Excluido { get; set; }
    }
}
