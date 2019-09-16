using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWorkflowAprovacao : IRepositorioBase<WorkflowAprovacao>
    {
        WorkflowAprovacao ObterEntidadeCompleta(long workflowId = 0, long notificacaoId = 0);

        IEnumerable<WorkflowAprovacao> ObterNiveisPorCodigo(string codigo);
    }
}