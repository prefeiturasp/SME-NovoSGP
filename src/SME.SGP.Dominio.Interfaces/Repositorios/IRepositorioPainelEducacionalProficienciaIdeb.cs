using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalProficienciaIdeb
    {
        Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>> ObterConsolidacaoPorAnoVisaoUeAsync(int limiteAnoLetivo, int anoLetivo, string codigoUe);
    }
}
