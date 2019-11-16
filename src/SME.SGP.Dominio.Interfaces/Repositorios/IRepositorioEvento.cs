using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEvento : IRepositorioBase<Evento>
    {
        bool EhEventoLetivoPorTipoDeCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId);

        bool ExisteEventoNaMesmaDataECalendario(DateTime dataInicio, long tipoCalendarioId);

        bool ExisteEventoPorEventoTipoId(long eventoTipoId);

        bool ExisteEventoPorFeriadoId(long feriadoId);

        bool ExisteEventoPorTipoCalendarioId(long tipoCalendarioId);

        Task<PaginacaoResultadoDto<Evento>> Listar(long? tipoCalendarioId, long? tipoEventoId, string nomeEvento, DateTime? dataInicio, DateTime? dataFim, Paginacao paginacao, string dreId, string ueId, Usuario usuario, Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor);

        Task<IEnumerable<CalendarioEventosNoDiaRetornoDto>> ObterEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes, int dia);

        Task<IEnumerable<Evento>> ObterEventosPorRecorrencia(long eventoId, long eventoPaiId, DateTime dataEvento);

        IEnumerable<Evento> ObterEventosPorTipoDeCalendarioDreUe(long tipoCalendarioId, string dreId, string ueId);

        Task<IEnumerable<Evento>> ObterEventosPorTipoETipoCalendario(long tipoEventoCodigo, long tipoCalendarioId);

        Evento ObterPorWorkflowId(long workflowId);

        Task<IEnumerable<EventosPorDiaRetornoQueryDto>> ObterQuantidadeDeEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes,
            Usuario usuario, Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor);

        Task<IEnumerable<CalendarioEventosMesesDto>> ObterQuantidadeDeEventosPorMeses(CalendarioEventosFiltroDto calendarioEventosMesesFiltro);

        Task<bool> TemEventoNosDiasETipo(DateTime dataInicio, DateTime dataFim, TipoEventoEnum liberacaoExcepcional, long tipoCalendarioId, string UeId, string DreId);
    }
}