using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosWorkflowAprovacao
    {
        Task Aprovar(bool aprovar, long notificacaoId, string observacao);

        Task ExcluirAsync(long idWorkflowAprovacao);

        Task<long> Salvar(WorkflowAprovacaoDto workflowAprovacaoNiveisDto);
    }
}