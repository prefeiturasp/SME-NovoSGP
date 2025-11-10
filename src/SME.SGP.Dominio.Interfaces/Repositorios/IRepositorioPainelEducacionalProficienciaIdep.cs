using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalProficienciaIdep
    {
        Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdepUe>> ObterConsolidacaoPorAnoVisaoUeAsync(int limiteAnoLetivo, int anoLetivo, string codigoUe);
    }
}
