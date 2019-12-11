using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAtribuicoes
    {
        Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(string professorRf);

        Task<IEnumerable<TurmaRetornoDto>> ObterUes(string professorRf, string codigoDre);
    }
}