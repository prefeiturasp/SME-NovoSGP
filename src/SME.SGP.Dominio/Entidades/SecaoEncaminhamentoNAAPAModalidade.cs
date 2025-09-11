using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class SecaoEncaminhamentoNAAPAModalidade : EntidadeBase
    {
        public long SecaoEncaminhamentoNAAPAId { get; set; }
        public Modalidade Modalidade { get; set; }
        public bool Excluido { get; set; }
    }
}
