using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class EncaminhamentoNAAPAMap : BaseMap<EncaminhamentoNAAPA>
    {
        public EncaminhamentoNAAPAMap()
        {
            ToTable("encaminhamento_naapa");
            Map(nameof(EncaminhamentoNAAPA.TurmaId), "turma_id");
            Map(nameof(EncaminhamentoNAAPA.AlunoCodigo), "aluno_codigo");
            Map(nameof(EncaminhamentoNAAPA.AlunoNome), "aluno_nome");
            Map(nameof(EncaminhamentoNAAPA.Situacao), "situacao");
            Map(nameof(EncaminhamentoNAAPA.Excluido), "excluido");
            Map(nameof(EncaminhamentoNAAPA.SituacaoMatriculaAluno), "situacao_matricula_aluno");
            Map(nameof(EncaminhamentoNAAPA.MotivoEncerramento), "motivo_encerramento");
            Map(nameof(EncaminhamentoNAAPA.DataUltimaNotificacaoSemAtendimento), "data_ultima_notificacao_sem_atendimento");
        }
    }
}