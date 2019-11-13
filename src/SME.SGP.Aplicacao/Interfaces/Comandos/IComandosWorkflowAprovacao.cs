using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IComandosWorkflowAprovacao
    {
        void Aprovar(bool aprovar, long notificacaoId, string observacao);

        long Salvar(WorkflowAprovacaoDto workflowAprovacaoNiveisDto);
    }
}