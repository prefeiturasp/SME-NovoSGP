using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio.Entidades
{
    [ExcludeFromCodeCoverage]
    public class ConselhoClasseParecer: EntidadeBase
    {
        public string Nome { get; set; }
        public bool Aprovado { get; set; }
        public bool Frequencia { get; set; }
        public bool Conselho { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime? FimVigencia { get; set; }
        public bool Nota { get; set; }
    }
}
