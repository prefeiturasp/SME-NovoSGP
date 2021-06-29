using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno
    {
        Task<long> Inserir(ConsolidacaoAcompanhamentoAprendizagemAluno consolidacao);

        Task Limpar();
        Task<IEnumerable<DashboardAcompanhamentoAprendizagemDto>> ObterConsolidacao(int anoLetivo, long dreId, long ueId, int semestre);
    }
}
