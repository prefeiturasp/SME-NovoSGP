using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoFinal : IRepositorioBase<FechamentoFinal>
    {
        Task<IEnumerable<FechamentoFinal>> ObterPorFiltros(string turmaCodigo, string[] disciplinasCodigo);
    }
}