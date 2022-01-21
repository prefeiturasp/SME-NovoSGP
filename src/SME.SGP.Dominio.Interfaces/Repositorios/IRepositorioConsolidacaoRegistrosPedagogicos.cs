using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidacaoRegistrosPedagogicos
    {
        Task<bool> ExisteConsolidacaoRegistroPedagogicoPorAno(int ano);
        Task ExcluirPorAno(int anoLetivo);
        Task Excluir(ConsolidacaoRegistrosPedagogicos consolidacao);
        Task<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>> GerarRegistrosPedagogicos(long ueId, int anoLetivo);
        Task<long> Inserir(ConsolidacaoRegistrosPedagogicos consolidacao);
    }
}
