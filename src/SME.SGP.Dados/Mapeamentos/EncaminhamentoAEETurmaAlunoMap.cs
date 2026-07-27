using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EncaminhamentoAEETurmaAlunoMap : BaseEntityMap<EncaminhamentoAEETurmaAluno>
    {
        public EncaminhamentoAEETurmaAlunoMap()
        {
            ToTable("encaminhamento_aee_turma_aluno");
            Map(nameof(EncaminhamentoAEETurmaAluno.EncaminhamentoAEEId), "encaminhamento_aee_id");
            Map(nameof(EncaminhamentoAEETurmaAluno.TurmaId), "turma_id");
            Map(nameof(EncaminhamentoAEETurmaAluno.AlunoCodigo), "aluno_codigo");
        }
    }
}