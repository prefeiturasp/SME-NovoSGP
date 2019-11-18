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
        Task<AbrangenciaFiltroRetorno> ObterAbrangenciaTurma(int turma);

        Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(Modalidade? modalidade, int periodo = 0);

        Task<IEnumerable<EnumeradoRetornoDto>> ObterModalidades();

        Task<IEnumerable<int>> ObterSemestres(Modalidade modalidade);

        Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, Modalidade modalidade, int periodo = 0);

        Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, Modalidade? modalidade, int periodo = 0);
    }
}