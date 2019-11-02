using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAbrangencia
    {
        Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorfiltro(string texto);

        Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(Modalidade? modalidade);

        Task<IEnumerable<EnumeradoRetornoDto>> ObterModalidades();

        Task<IEnumerable<int>> ObterSemestres(Modalidade modalidade);

        Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, Modalidade modalidade);

        Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, Modalidade? modalidade);
    }
}