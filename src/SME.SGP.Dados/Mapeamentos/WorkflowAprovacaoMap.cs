using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoMap : BaseMap<WorkflowAprovacao>
    {
        public WorkflowAprovacaoMap()
        {
            ToTable("wf_aprovacao");
            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.NotifacaoMensagem).ToColumn("notifacao_mensagem");
            Map(c => c.NotifacaoTitulo).ToColumn("notifacao_titulo");
            Map(c => c.NotificacaoCategoria).ToColumn("notificacao_categoria");
            Map(c => c.NotificacaoTipo).ToColumn("notificacao_tipo");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.UeId).ToColumn("ue_id");
        }
    }
}