using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEvento : IRepositorioBase<Evento>
    {
        bool ExisteEventoNaMesmaDataECalendario(DateTime dataInicio, long tipoCalendarioId);
        bool EhEventoLetivoPorTipoDeCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId);

        Task<PaginacaoResultadoDto<Evento>> Listar(long? tipoCalendarioId, long? tipoEventoId, string nomeEvento, DateTime? dataInicio, DateTime? dataFim, Paginacao paginacao);

        Task<IEnumerable<CalendarioEventosNoDiaRetornoDto>> ObterEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes, int dia);

        Task<IEnumerable<Evento>> ObterEventosPorRecorrencia(long eventoId, long eventoPaiId, DateTime dataEvento);

        Task<IEnumerable<Evento>> ObterEventosPorTipoETipoCalendario(long tipoEventoCodigo, long tipoCalendarioId);

        Task<IEnumerable<EventosPorDiaRetornoQueryDto>> ObterQuantidadeDeEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes);

        IEnumerable<Evento> ObterEventosPorTipoDeCalendarioDreUe(long tipoCalendarioId, string dreId, string ueId);

        Task<IEnumerable<CalendarioEventosMesesDto>> ObterQuantidadeDeEventosPorMeses(CalendarioEventosFiltroDto calendarioEventosMesesFiltro);
        bool ExisteEventoPorTipoCalendarioId(long tipoCalendarioId);
        bool ExisteEventoPorFeriadoId(long feriadoId);
        bool ExisteEventoPorEventoTipoId(long eventoTipoId);
    }
}