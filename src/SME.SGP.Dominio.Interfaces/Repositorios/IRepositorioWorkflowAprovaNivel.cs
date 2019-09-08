using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWorkflowAprovaNivel : IRepositorioBase<WorkflowAprovacao>
    {
        IEnumerable<WorkflowAprovacao> ObterNiveisPorCodigo(string codigo);
    }
}