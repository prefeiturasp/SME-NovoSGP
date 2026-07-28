using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class AnotacaoFechamentoAluno : EntidadeBase
    {
        [Computed]
        public FechamentoAluno FechamentoAluno { get; set; }
        public long FechamentoAlunoId { get; set; }
        public string Anotacao { get; set; }
    }
}
