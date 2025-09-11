using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ComunicadoModalidade
    {
        public ComunicadoModalidade()
        {
        }
        public long ComunicadoId { get; set; }
        public long Modalidade { get; set; }
        public long Id { get; set; }
    }
}
