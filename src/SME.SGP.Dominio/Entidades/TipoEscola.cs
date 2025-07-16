using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class TipoEscolaEol : EntidadeBase
    {
        public int CodEol { get; set; }
        public string Descricao { get; set; }
        public DateTime DtAtualizacao { get; set; }
    }
}