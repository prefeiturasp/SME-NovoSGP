using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWfAprovacaoNotaFechamento
    {
        Task SalvarAsync(WfAprovacaoNotaFechamento entidade);
        Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterPorNotaId(long fechamentoNotaId);
        Task Excluir(WfAprovacaoNotaFechamento wfAprovacaoNota);
    }
}
