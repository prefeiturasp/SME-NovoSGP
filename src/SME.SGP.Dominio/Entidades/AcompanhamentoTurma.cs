using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class AcompanhamentoTurma : EntidadeBase
    {
        [Computed]
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }

        public int Semestre { get; set; }
        public string ApanhadoGeral { get; set; }
    }
}
