using SME.SGP.Dominio;

namespace SME.SGP.Dados
{ 
    public class EncaminhamentoNAAPAMap : BaseMap<EncaminhamentoNAAPA>
    {
        public EncaminhamentoNAAPAMap()
        {
            ToTable("encaminhamento_naapa");

            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.AlunoNome).ToColumn("aluno_nome");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.SituacaoMatriculaAluno).ToColumn("situacao_matricula_aluno");
            Map(c => c.MotivoEncerramento).ToColumn("motivo_encerramento");
            Map(c => c.DataUltimaNotificacaoSemAtendimento).ToColumn("data_ultima_notificacao_sem_atendimento");
        }
    }
}
