using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAbandono
    {
        Task<IEnumerable<PainelEducacionalAbandono>> ObterAbandonoVisaoSmeDre(int anoLetivo, string codigoDre);
    }
}
