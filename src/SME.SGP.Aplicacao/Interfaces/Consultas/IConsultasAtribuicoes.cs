using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAtribuicoes
    {
        Task<IEnumerable<int>> ObterAnosLetivos(bool consideraHistorico);
        Task<IEnumerable<AbrangenciaDreRetornoDto>> ObterDres(int anoLetivo, bool consideraHistorico);
        Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, int anoLetivo, bool consideraHistorico);
    }
}