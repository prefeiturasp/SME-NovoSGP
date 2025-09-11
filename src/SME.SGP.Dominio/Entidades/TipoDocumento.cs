using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class TipoDocumento
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
    }
}