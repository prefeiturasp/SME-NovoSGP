using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWorkflowAprovacaoNivelUsuario
    {
        void Salvar(WorkflowAprovacaoNivelUsuario workflowAprovaNivelUsuario);
        Task SalvarAsync(WorkflowAprovacaoNivelUsuario workflowAprovaNivelUsuario);
    }
}