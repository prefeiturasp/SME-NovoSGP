using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EncaminhamentoAEETurmaAlunoMap : BaseMap<EncaminhamentoAEETurmaAluno>
    {
        public EncaminhamentoAEETurmaAlunoMap()
        {
            ToTable("encaminhamento_aee_turma_aluno");
            Map(c => c.EncaminhamentoAEEId).ToColumn("encaminhamento_aee_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
        }
    }
}
