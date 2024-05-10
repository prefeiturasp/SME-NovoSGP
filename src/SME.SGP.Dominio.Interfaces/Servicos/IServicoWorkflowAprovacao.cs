using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoWorkflowAprovacao
    {
        Task Aprovar(WorkflowAprovacao workflow, bool aprovar, string observacao, long notificacaoId);

        Task ConfiguracaoInicial(WorkflowAprovacao workflowAprovacao, long idEntidadeParaAprovar);
        Task ConfiguracaoInicialAsync(WorkflowAprovacao workflowAprovacao, long idEntidadeParaAprovar);

        Task ExcluirWorkflowNotificacoes(long id);

        Task<string> VerificaAulaReposicao(long workflowId, long codigoDaNotificacao);

    }
}