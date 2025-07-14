using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConselhoClasseAlunoRecomendacao 
    {
        public long Id { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        public ConselhoClasseAluno ConselhoClasseAluno { get; set; }
        public long ConselhoClasseRecomendacaoId { get; set; }
        public ConselhoClasseRecomendacao ConselhoClasseRecomendacao { get; set; }
    }
}
