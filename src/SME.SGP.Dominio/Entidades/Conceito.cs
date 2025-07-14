using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class Conceito : EntidadeBase
    {
        public bool Aprovado { get; set; }
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
        public DateTime FimVigencia { get; set; }
        public DateTime InicioVigencia { get; set; }
        public string Valor { get; set; }
    }
}