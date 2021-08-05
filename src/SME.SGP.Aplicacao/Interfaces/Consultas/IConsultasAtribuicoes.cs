using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAtribuicoes
    {
        Task<IEnumerable<AbrangenciaDreRetornoDto>> ObterDres(int anoLetivo);

        Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, int anoLetivo);
    }
}