using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class FechamentoNota : EntidadeBase
    {
        public long FechamentoAlunoId { get; set; }
        public FechamentoAluno FechamentoAluno { get; set; }
        public long DisciplinaId { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public Conceito Conceito { get; set; }
        public long? SinteseId { get; set; }
        public Sintese Sintese { get; set; }

        public bool Migrado { get; set; }
        public bool Excluido { get; set; }

        public FechamentoNota Clone()
        {
            return (FechamentoNota)this.MemberwiseClone();
        }
    }
}