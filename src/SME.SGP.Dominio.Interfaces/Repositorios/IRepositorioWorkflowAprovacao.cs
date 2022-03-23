using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWorkflowAprovacao : IRepositorioBase<WorkflowAprovacao>
    {
        WorkflowAprovacao ObterEntidadeCompleta(long workflowId = 0, long notificacaoId = 0);

        Task<WorkflowAprovacao> ObterEntidadeCompletaPorId(long workflowId);

        IEnumerable<WorkflowAprovacao> ObterNiveisPorCodigo(string codigo);
        Task<string> ObterCriador(long workflowId);
    }
}