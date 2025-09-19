using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalIdeb
    {
        Task<IEnumerable<PainelEducacionalIdebDto>> ObterIdeb();
        Task<bool> SalvarIdeb(IEnumerable<PainelEducacionalConsolidacaoIdeb> ideb);
        Task ExcluirIdeb();
    }
}
