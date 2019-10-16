using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAbrangencia
    {
        Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorfiltro(string texto);

        Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres();

        Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe);

        Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre);
    }
}