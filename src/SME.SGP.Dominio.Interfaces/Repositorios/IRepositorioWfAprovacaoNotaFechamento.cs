
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWfAprovacaoNotaFechamento
    {
        Task SalvarAsync(WfAprovacaoNotaFechamento entidade);
        Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterPorNotaId(long fechamentoNotaId);
        Task Excluir(WfAprovacaoNotaFechamento wfAprovacaoNota);
        Task ExcluirLogico(WfAprovacaoNotaFechamento wfAprovacaoNota);
        Task<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>> ObterWfAprovacaoNotaFechamentoSemWfAprovacaoId();
        Task<bool> AlterarWfAprovacaoNotaFechamentoComWfAprovacaoId(long workflowAprovacaoId, long[] workflowAprovacaoNotaFechamentoIds);
        Task<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>> ObterWfAprovacaoNotaFechamentoComWfAprovacaoId(long workflowId);
    }
}
