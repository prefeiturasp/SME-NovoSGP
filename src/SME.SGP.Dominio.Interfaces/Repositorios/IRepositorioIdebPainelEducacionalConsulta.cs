using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioIdebPainelEducacionalConsulta
    {
        Task<IEnumerable<PainelEducacionalIdebDto>> ObterIdebPorAnoSerie(int anoLetivo, int serie, string codigoDre, string codigoUe);
        Task<int?> ObterAnoMaisRecenteIdeb(int serie, string codigoDre, string codigoUe);
    }
}
