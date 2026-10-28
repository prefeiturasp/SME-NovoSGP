using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoAEETurmaAlunoMap : BaseMap<PlanoAEETurmaAluno>
    {
        public PlanoAEETurmaAlunoMap()
        {
            ToTable("plano_aee_turma_aluno");
            Map(nameof(PlanoAEETurmaAluno.PlanoAEEId), "plano_aee_id");
            Map(nameof(PlanoAEETurmaAluno.TurmaId), "turma_id");
            Map(nameof(PlanoAEETurmaAluno.AlunoCodigo), "aluno_codigo");
        }
    }
}