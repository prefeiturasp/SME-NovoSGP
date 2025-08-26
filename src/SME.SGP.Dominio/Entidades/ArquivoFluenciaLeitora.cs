using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ArquivoFluenciaLeitora : EntidadeBase
    {
        public string CodigoEOLTurma{ get; set; }
        public string CodigoEOLAluno { get; set; }
        public int Fluencia { get; set; }
        public string Periodo{ get; set; }
    }
}
