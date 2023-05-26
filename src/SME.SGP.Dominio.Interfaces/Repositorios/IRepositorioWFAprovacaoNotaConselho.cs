using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWFAprovacaoNotaConselho : IRepositorioBase<WFAprovacaoNotaConselho>
    {
        Task<IEnumerable<WFAprovacaoNotaConselho>> ObterNotasEmAprovacaoPorWorkflow(long workflowId);
        Task<IEnumerable<WFAprovacaoNotaConselho>> ObterWfsAprovacaoPorWorkflow(long workflowId);
        Task<IEnumerable<WFAprovacaoNotaConselho>> ObterWorkflowAprovacaoNota(long conselhoClasseNotaId);
        Task<IEnumerable<WFAprovacaoNotaConselho>> ObterNotasAguardandoAprovacaoSemWorkflow();
        Task ExcluirLogico(WFAprovacaoNotaConselho wfAprovacaoNota);
    }
}
