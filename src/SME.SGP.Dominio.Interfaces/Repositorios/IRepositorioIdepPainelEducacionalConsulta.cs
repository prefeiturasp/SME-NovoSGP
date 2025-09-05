using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioIdepPainelEducacionalConsulta : IRepositorioBase<PainelEducacionalIdep>
    {
        Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> ObterTodosIdep();
    }
}
