using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAbrangencia
    {
        Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorfiltro(string texto, bool consideraHistorico, bool consideraNovosAnosInfantil = false);

        Task<AbrangenciaFiltroRetorno> ObterAbrangenciaTurma(string turma, bool consideraHistorico = false);

        Task<IEnumerable<OpcaoDropdownDto>> ObterAnosTurmasPorUeModalidade(string codigoUe, Modalidade modalidade, bool consideraHistorico);

        Task<IEnumerable<int>> ObterAnosLetivos(bool consideraHistorico, int anoMinimo);

        Task<IEnumerable<int>> ObterAnosLetivosTodos();

        Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0);

        Task<IEnumerable<int>> ObterSemestres(Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0);

        Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasRegulares(string codigoUe, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0);

        Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasPrograma(string codigoUe, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0);

        Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, bool consideraNovasUEs = false);
        Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, Modalidade modalidade, int periodo, bool consideraHistorico, int anoLetivo, int[] tipos, bool desconsideraNovosAnosInfantil = false);
    }
}