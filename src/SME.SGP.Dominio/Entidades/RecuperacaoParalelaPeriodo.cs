using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RecuperacaoParalelaPeriodo : EntidadeBase
    {
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public string Nome { get; set; }
        public int BimestreEdicao { get; set; }
    }
}