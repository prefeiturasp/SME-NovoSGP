using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PlanoAEEReestruturacao : EntidadeBase
    {
        [Computed]
        public PlanoAEEVersao PlanoAEEVersao { get; set; }
        public long PlanoAEEVersaoId { get; set; }
        public int Semestre { get; set; }
        public string Descricao { get; set; }
    }
}
