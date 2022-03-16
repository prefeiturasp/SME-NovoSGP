using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoAlunoMap: BaseMap<FechamentoAluno>
    {
        public FechamentoAlunoMap()
        {
            ToTable("fechamento_aluno");
            Map(c => c.FechamentoTurmaDisciplinaId).ToColumn("fechamento_turma_disciplina_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
