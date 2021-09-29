using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWFAprovacaoNotaConselho
    {
        Task Salvar(WFAprovacaoNotaConselho entidade);
        Task<WFAprovacaoNotaConselho> ObterNotaEmAprovacao(long workflowId);
        Task Excluir(long id);
    }
}
