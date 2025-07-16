using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class AcompanhamentoTurma : EntidadeBase
    {
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }

        public int Semestre { get; set; }
        public string ApanhadoGeral { get; set; }
    }
}
