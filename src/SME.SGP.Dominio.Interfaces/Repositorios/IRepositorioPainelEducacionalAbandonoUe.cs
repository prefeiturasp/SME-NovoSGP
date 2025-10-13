using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAbandonoUe
    {
        Task<IEnumerable<PainelEducacionalAbandonoUe>> ObterAbandonoUe(int anoLetivo, string codigoDre, string codigoUe, string modalidade, int numeroPagina, int numeroRegistros);
    }
}