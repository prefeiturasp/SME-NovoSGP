using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class GrupoComunicacao : EntidadeBase
    {
        public string Nome { get; set; }
        public string TipoCicloId { get; set; }
        public string TipoEscolaId { get; set; }
        public string EtapaEnsino { get; set; }
    }
}