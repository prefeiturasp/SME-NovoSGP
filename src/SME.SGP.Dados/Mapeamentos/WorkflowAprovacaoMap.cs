using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WorkflowAprovacaoMap : BaseMap<WorkflowAprovacao>
    {
        public WorkflowAprovacaoMap()
        {
            ToTable("wf_aprovacao");
            Map(nameof(WorkflowAprovacao.Ano), "ano");
            Map(nameof(WorkflowAprovacao.Excluido), "excluido");
            Map(nameof(WorkflowAprovacao.DreId), "dre_id");
            Map(nameof(WorkflowAprovacao.NotifacaoMensagem), "notificacao_mensagem");
            Map(nameof(WorkflowAprovacao.NotifacaoTitulo), "notificacao_titulo");
            Map(nameof(WorkflowAprovacao.NotificacaoCategoria), "notificacao_categoria");
            Map(nameof(WorkflowAprovacao.NotificacaoTipo), "notificacao_tipo");
            Map(nameof(WorkflowAprovacao.Tipo), "tipo");
            Map(nameof(WorkflowAprovacao.TurmaId), "turma_id");
            Map(nameof(WorkflowAprovacao.UeId), "ue_id");
        }
    }
}