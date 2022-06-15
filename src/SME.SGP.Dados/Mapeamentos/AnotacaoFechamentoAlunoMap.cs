using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AnotacaoFechamentoAlunoMap : BaseMap<AnotacaoFechamentoAluno>
    {
        public AnotacaoFechamentoAlunoMap()
        {
            ToTable("anotacao_fechamento_aluno");
            Map(a => a.FechamentoAlunoId).ToColumn("fechamento_aluno_id");
            Map(a => a.Anotacao).ToColumn("anotacao");
        }
    }
}
