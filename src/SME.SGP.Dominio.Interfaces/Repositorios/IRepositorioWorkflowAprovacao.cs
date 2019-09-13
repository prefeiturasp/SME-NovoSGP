using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWorkflowAprovacao : IRepositorioBase<WorkflowAprovacao>
    {
        IEnumerable<WorkflowAprovacao> ObterNiveisPorCodigo(string codigo);
    }
}