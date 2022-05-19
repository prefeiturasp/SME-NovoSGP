using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoAlunoMap : BaseMap<AcompanhamentoAluno>
    {
        public AcompanhamentoAlunoMap()
        {
            ToTable("acompanhamento_aluno");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
