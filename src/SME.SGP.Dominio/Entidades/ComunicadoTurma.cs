using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio.Entidades
{
    [ExcludeFromCodeCoverage]
    public class ComunicadoTurma : EntidadeBase
    {
        public string CodigoTurma { get; set; }
        public long ComunicadoId { get; set; }
        public bool Excluido { get; set; }
    }
}
