using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class CicloEnsino : EntidadeBase
    {
        public int CodEol { get; set; }
        public string Descricao { get; set; }
        public DateTime DtAtualizacao { get; set; }
        public int CodigoModalidadeEnsino { get; set; }
        public int CodigoEtapaEnsino { get; set; }
    }
}