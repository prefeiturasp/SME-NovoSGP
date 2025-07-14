using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RegistroPoa : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public string CodigoRf { get; set; }
        public string Descricao { get; set; }
        public string DreId { get; set; }
        public bool Excluido { get; set; }
        public string Titulo { get; set; }
        public string UeId { get; set; }
    }
}