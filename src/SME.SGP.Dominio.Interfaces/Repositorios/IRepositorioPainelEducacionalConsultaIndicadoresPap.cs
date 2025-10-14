using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsultaIndicadoresPap
    {
        Task<IEnumerable<PainelEducacionalConsolidacaoPapBase>> ObterConsolidacoesSmePorAno(int ano);
        Task<IEnumerable<PainelEducacionalConsolidacaoPapBase>> ObterConsolidacoesDrePorAno(int ano, string codigoDre);
        Task<IEnumerable<PainelEducacionalConsolidacaoPapBase>> ObterConsolidacoesUePorAno(int ano, string codigoDre, string codigoUe);
    }
}