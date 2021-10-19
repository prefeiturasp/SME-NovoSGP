using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidacaoRegistrosPedagogicos
    {
        Task<bool> ExisteConsolidacaoRegistroPedagogicoPorAno(int ano);
        Task ExcluirPorAno(int anoLetivo);
        Task<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>> GerarRegistrosPedagogicos(long ueId, int anoLetivo);
        Task<long> Inserir(ConsolidacaoRegistrosPedagogicos consolidacao);
    }
}
