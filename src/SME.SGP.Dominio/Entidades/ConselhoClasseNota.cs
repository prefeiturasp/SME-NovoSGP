using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConselhoClasseNota: EntidadeBase
    {
        public ConselhoClasseAluno ConselhoClasseAluno { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public Conceito Conceito { get; set; }
        public string Justificativa { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }

        public ConselhoClasseNota Clone()
        {
            return (ConselhoClasseNota)this.MemberwiseClone();
        }
    }
}
