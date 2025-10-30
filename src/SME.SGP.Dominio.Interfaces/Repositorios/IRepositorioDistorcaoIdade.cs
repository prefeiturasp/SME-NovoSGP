using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioDistorcaoIdade
    {
        Task<IEnumerable<ConsolidacaoDistorcaoIdadeDto>> ObterDistorcaoIdade(FiltroPainelEducacionalDistorcaoIdade filtro);
    }
}
