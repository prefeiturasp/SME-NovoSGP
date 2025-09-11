using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class OcorrenciaTipo : EntidadeBase
    {
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
    }
}