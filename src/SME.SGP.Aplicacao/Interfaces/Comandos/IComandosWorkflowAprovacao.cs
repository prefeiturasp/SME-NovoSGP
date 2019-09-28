using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public interface IComandosWorkflowAprovacao
    {
        void Aprovar(bool aprovar, long notificacaoId, string observacao);

        void Salvar(WorkflowAprovacaoDto workflowAprovacaoNiveisDto);
    }
}