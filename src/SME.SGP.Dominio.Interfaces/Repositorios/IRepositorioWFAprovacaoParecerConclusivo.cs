using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWFAprovacaoParecerConclusivo : IRepositorioBase<WFAprovacaoParecerConclusivo>
    {
        Task<WFAprovacaoParecerConclusivo> ObterPorWorkflowId(long workflowId);
        Task<IEnumerable<WFAprovacaoParecerConclusivo>> ObterPorConselhoClasseAlunoId(long conselhoClasseAlunoId);
        Task Excluir(long id);
        Task<WFAprovacaoParecerConclusivoDto> ObterAprovacaoParecerConclusivoPorWorkflowId(long workflowId);
    }
}
