using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoMap : BaseMap<WorkflowAprovacao>
    {
        public WorkflowAprovacaoMap()
        {
            ToTable("wf_aprovacao");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.NotifacaoMensagem).ToColumn("notificacao_mensagem");
            Map(c => c.NotifacaoTitulo).ToColumn("notificacao_titulo");
            Map(c => c.NotificacaoTipo).ToColumn("notificacao_tipo");
            Map(c => c.NotificacaoCategoria).ToColumn("notificacao_categoria");
        }
    }
}