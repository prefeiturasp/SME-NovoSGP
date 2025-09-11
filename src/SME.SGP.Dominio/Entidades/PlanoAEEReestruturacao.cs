using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PlanoAEEReestruturacao : EntidadeBase
    {
        public PlanoAEEVersao PlanoAEEVersao { get; set; }
        public long PlanoAEEVersaoId { get; set; }
        public int Semestre { get; set; }
        public string Descricao { get; set; }
    }
}
