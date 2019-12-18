using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasEvento
    {
        Task<PaginacaoResultadoDto<EventoCompletoDto>> Listar(FiltroEventosDto filtroEventosDto);

        Task<IEnumerable<CalendarioEventosNoDiaRetornoDto>> ObterEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes, int dia);

        Task<EventoCompletoDto> ObterPorId(long id);

        Task<IEnumerable<CalendarioTipoEventoPorDiaDto>> ObterQuantidadeDeEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes);

        Task<IEnumerable<CalendarioEventosMesesDto>> ObterQuantidadeDeEventosPorMeses(CalendarioEventosFiltroDto calendarioEventosMesesFiltro);
    }
}