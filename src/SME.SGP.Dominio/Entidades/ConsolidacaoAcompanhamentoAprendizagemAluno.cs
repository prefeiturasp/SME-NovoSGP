using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConsolidacaoAcompanhamentoAprendizagemAluno
    {
        public ConsolidacaoAcompanhamentoAprendizagemAluno(long turmaId, int quantidadeComAcompanhamento, int quantidadeSemAcompanhamento, int semestre)
        {
            TurmaId = turmaId;
            QuantidadeComAcompanhamento = quantidadeComAcompanhamento;
            QuantidadeSemAcompanhamento = quantidadeSemAcompanhamento;
            Semestre = semestre;
        }

        public long Id { get; set; }
        public long TurmaId { get; set; }
        public int QuantidadeComAcompanhamento { get; set; }
        public int QuantidadeSemAcompanhamento { get; set; }
        public int Semestre { get; set; }
    }
}
