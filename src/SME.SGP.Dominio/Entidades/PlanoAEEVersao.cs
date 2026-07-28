using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PlanoAEEVersao : EntidadeBase
    {
        public long PlanoAEEId { get; set; }
        [Computed]
        public PlanoAEE PlanoAEE { get; set; }

        public int Numero { get; set; }

        public bool Excluido { get; set; }
    }
}
