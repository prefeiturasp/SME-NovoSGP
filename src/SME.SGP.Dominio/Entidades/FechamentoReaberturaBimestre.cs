using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class FechamentoReaberturaBimestre : EntidadeBase
    {
        public int Bimestre { get; set; }
        public FechamentoReabertura FechamentoAbertura { get; set; }
        public long FechamentoAberturaId { get; set; }
    }
}