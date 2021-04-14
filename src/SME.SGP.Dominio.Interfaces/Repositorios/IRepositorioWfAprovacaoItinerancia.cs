using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWfAprovacaoItinerancia
    {
        Task SalvarAsync(WfAprovacaoItinerancia entidade);

        Task<WfAprovacaoItinerancia> ObterPorWorkflowId(long workflowId);
        Task<WfAprovacaoItinerancia> ObterPorItineranciaId(long itineranciaId);
    }
}
