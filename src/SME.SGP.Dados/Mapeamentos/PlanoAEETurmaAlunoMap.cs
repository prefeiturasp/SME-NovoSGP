using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoAEETurmaAlunoMap : BaseMap<PlanoAEETurmaAluno>
    {
        public PlanoAEETurmaAlunoMap()
        {
            ToTable("plano_aee_turma_aluno");
            Map(c => c.PlanoAEEId).ToColumn("plano_aee_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
        }
    }
}
