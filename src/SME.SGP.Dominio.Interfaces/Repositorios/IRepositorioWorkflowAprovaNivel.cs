using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWorkflowAprovaNivel : IRepositorioBase<WorkflowAprovaNivel>
    {
        IEnumerable<WorkflowAprovaNivel> ObterNiveisPorCodigo(string codigo);
    }
}