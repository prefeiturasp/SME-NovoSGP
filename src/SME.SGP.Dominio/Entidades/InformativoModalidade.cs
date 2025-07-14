using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class InformativoModalidade : EntidadeBase
    {
        public long InformativoId { get; set; }
        public Modalidade Modalidade { get; set; }
        public Informativo Informativo { get; set; }
    }
}
