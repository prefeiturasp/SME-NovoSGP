using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class ConselhoClasseAlunoRecomendacao 
    {
        [Key]
        public long Id { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        [Computed]
        public ConselhoClasseAluno ConselhoClasseAluno { get; set; }
        public long ConselhoClasseRecomendacaoId { get; set; }
        [Computed]
        public ConselhoClasseRecomendacao ConselhoClasseRecomendacao { get; set; }
    }
}
