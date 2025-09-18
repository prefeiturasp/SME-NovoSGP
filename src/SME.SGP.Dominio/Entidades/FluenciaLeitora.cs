using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class FluenciaLeitora : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public string CodigoEOLTurma{ get; set; }
        public string CodigoEOLAluno { get; set; }
        public int Fluencia { get; set; }
        public int TipoAvaliacao { get; set; }
    }
}
