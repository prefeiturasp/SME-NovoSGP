using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasEvento
    {
        Task<PaginacaoResultadoDto<EventoCompletoDto>> Listar(FiltroEventosDto filtroEventosDto);

        EventoCompletoDto ObterPorId(long id);

        Task<IEnumerable<CalendarioEventosMesesDto>> ObterQuantidadeDeEventosPorMeses(CalendarioEventosMesesFiltroDto calendarioEventosMesesFiltro);
    }
}