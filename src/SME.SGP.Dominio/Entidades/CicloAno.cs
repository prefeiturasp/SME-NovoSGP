using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class CicloAno
    {
        public long Id { get; set; }
        public Ciclo Ciclo { get; set; }
        public long CicloId { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
    }
}
