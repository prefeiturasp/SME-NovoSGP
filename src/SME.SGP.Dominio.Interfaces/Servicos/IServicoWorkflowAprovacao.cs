namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoWorkflowAprovacao
    {
        void Aprovar(WorkflowAprovacao workflow, bool aprovar, string observacao, long notificacaoId);

        void ConfiguracaoInicial(WorkflowAprovacao workflowAprovacao);
    }
}