namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWorkflowAprovacaoNivelNotificacao
    {
        void Salvar(WorkflowAprovacaoNivelNotificacao workflowAprovaNivelNotificacao);

        void ExcluirPorWorkflowNivelNotificacaoId(long workflowNivelId, long notificacaoId);
    }
}