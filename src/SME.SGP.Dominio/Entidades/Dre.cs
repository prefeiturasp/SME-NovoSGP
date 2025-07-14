using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class Dre
    {
        public string Abreviacao { get; set; }
        public string CodigoDre { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }
    }
}