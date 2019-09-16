using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWorkflowAprovacao : IRepositorioBase<WorkflowAprovacao>
    {
        WorkflowAprovacao ObterComNiveisPorId(long id);

        IEnumerable<WorkflowAprovacao> ObterNiveisPorCodigo(string codigo);
    }
}