using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWFAprovacaoParecerConclusivo : IRepositorioBase<WFAprovacaoParecerConclusivo>
    {
        Task<WFAprovacaoParecerConclusivo> ObterPorWorkflowId(long workflowId);
        Task<WFAprovacaoParecerConclusivo> ObterPorConselhoClasseAlunoId(long conselhoClasseAlunoId);
        Task Excluir(long id);
        Task<WFAprovacaoParecerConclusivoDto> ObterAprovacaoParecerConclusivoPorWorkflowId(long workflowId);
    }
}
