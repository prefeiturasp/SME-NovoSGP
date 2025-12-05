using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EncaminhamentoEscolarMap : BaseMap<EncaminhamentoEscolar>
    {
        public EncaminhamentoEscolarMap()
        {
            ToTable("encaminhamento_escolar");

            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.AlunoNome).ToColumn("aluno_nome");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.SituacaoMatriculaAluno).ToColumn("situacao_matricula_aluno");
            Map(c => c.MotivoEncerramento).ToColumn("motivo_encerramento");
            Map(c => c.DataUltimaNotificacaoSemAtendimento).ToColumn("data_ultima_notificacao_sem_atendimento");
        }
    }
}