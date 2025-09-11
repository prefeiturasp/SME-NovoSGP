using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConselhoClasse: EntidadeBase
    {
        public ConselhoClasse()
        {
            Migrado = false;
            Excluido = false;
            Situacao = SituacaoConselhoClasse.EmAndamento;
        }

        public long FechamentoTurmaId { get; set; }
        public FechamentoTurma FechamentoTurma { get; set; }

        public SituacaoConselhoClasse Situacao { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
    }
}
