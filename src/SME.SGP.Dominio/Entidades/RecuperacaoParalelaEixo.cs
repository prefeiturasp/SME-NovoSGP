using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RecuperacaoParalelaEixo : EntidadeBase
    {
        public string Descricao { get; set; }
        public DateTime DtFim { get; set; }
        public DateTime DtInicio { get; set; }
        public bool Excluido { get; set; }
        public int RecuperacaoParalelaPeriodoId { get; set; }
    }
}