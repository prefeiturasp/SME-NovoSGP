using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RecuperacaoParalelaObjetivoDesenvolvimentoPlano : EntidadeBase
    {
        public long ObjetivoDesenvolvimentoId { get; set; }
        public long PlanoId { get; set; }
    }
}