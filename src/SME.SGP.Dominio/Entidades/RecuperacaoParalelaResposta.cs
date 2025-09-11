using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RecuperacaoParalelaResposta : EntidadeBase
    {
        public string Descricao { get; set; }
        public DateTime DtFim { get; set; }
        public DateTime DtInicio { get; set; }
        public bool Excluido { get; set; }
        public string Nome { get; set; }
        public bool Sim { get; set; }
    }
}