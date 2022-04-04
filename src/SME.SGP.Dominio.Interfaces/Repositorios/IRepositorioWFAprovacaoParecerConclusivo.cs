using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWFAprovacaoParecerConclusivo
    {
        Task Salvar(WFAprovacaoParecerConclusivo entidade);
        Task<WFAprovacaoParecerConclusivo> ObterPorWorkflowId(long workflowId);
        Task<WFAprovacaoParecerConclusivo> ObterPorConselhoClasseAlunoId(long conselhoClasseAlunoId);
        Task Excluir(long id);
        Task<WFAprovacaoParecerConclusivoDto> ObterAprovacaoParecerConclusivoPorWorkflowId(long workflowId);
    }
}
