using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoWorkflowAprovacao
    {
        Task Aprovar(WorkflowAprovacao workflow, bool aprovar, string observacao, long notificacaoId);

        void ConfiguracaoInicial(WorkflowAprovacao workflowAprovacao, long idEntidadeParaAprovar);

        Task ExcluirWorkflowNotificacoes(long id);
    }
}