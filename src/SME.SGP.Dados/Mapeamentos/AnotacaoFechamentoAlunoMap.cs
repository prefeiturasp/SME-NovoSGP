using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AnotacaoFechamentoAlunoMap : BaseMap<AnotacaoFechamentoAluno>
    {
        public AnotacaoFechamentoAlunoMap()
        {
            ToTable("anotacao_fechamento_aluno");
            Map(nameof(AnotacaoFechamentoAluno.FechamentoAlunoId), "fechamento_aluno_id");
            Map(nameof(AnotacaoFechamentoAluno.Anotacao), "anotacao");
        }
    }
}