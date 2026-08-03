using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoAlunoMap : BaseMap<AcompanhamentoAluno>
    {
        public AcompanhamentoAlunoMap()
        {
            ToTable("acompanhamento_aluno");
            Map(nameof(AcompanhamentoAluno.TurmaId), "turma_id");
            Map(nameof(AcompanhamentoAluno.AlunoCodigo), "aluno_codigo");
            Map(nameof(AcompanhamentoAluno.Excluido), "excluido");
        }
    }
}
