using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEvento : IRepositorioBase<Evento>
    {
        bool EhEventoNaoLetivoPorTipoDeCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId);
        Task<IEnumerable<Evento>> ObterEventosCalendarioProfessorPorMes(long tipoCalendarioId, string dreCodigo, string ueCodigo, int mes, bool VisualizarEventosSME = false, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = false);
        List<Evento> EhEventoLetivoPorLiberacaoExcepcional(long tipoCalendarioId, DateTime dataAula, string ueId);

        bool EhEventoLetivoPorTipoDeCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId);

        Task<IEnumerable<Evento>> EventosNosDiasETipo(DateTime dataInicio, DateTime dataFim, TipoEvento tipoEventoCodigo, long tipoCalendarioId, string UeId, string DreId, bool utilizarRangeDatas = true);        
        bool ExisteEventoPorEventoTipoId(long eventoTipoId);

        bool ExisteEventoPorFeriadoId(long feriadoId);

        bool ExisteEventoPorTipoCalendarioId(long tipoCalendarioId);
        Task<PaginacaoResultadoDto<Evento>> Listar(long? tipoCalendarioId, long? tipoEventoId, string nomeEvento, DateTime? dataInicio, DateTime? dataFim, Paginacao paginacao, string dreId, string ueId,
           bool ehTodasDres, bool ehTodasUes, Usuario usuario, Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor, bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, bool consideraHistorico);

        Task<IEnumerable<CalendarioEventosNoDiaRetornoDto>> ObterEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes, int dia, Usuario usuario);

        IEnumerable<Evento> ObterEventosPorRecorrencia(long eventoId, long eventoPaiId, DateTime dataEvento);
        
        IEnumerable<Evento> ObterEventosPorTipoDeCalendarioDreUe(long tipoCalendarioId, string dreId, string ueId, bool EhEventoSme = false, bool filtroDreUe = true, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = true);

        Task<IEnumerable<Evento>> ObterEventosPorTipoDeCalendarioDreUeDia(long tipoCalendarioId, string dreId, string ueId, DateTime data, bool EhEventoSme, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = true);

        Task<IEnumerable<Evento>> ObterEventosPorTipoDeCalendarioDreUeMes(long tipoCalendarioId, string dreId, string ueId, int mes, bool EhEventoSme, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = true);

        Task<IEnumerable<Evento>> ObterEventosPorTipoETipoCalendario(long tipoEventoCodigo, long tipoCalendarioId);

        Evento ObterPorWorkflowId(long workflowId);

        Task<IEnumerable<EventosPorDiaRetornoQueryDto>> ObterQuantidadeDeEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes,
            Usuario usuario, Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor, bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme);

        Task<IEnumerable<CalendarioEventosMesesDto>> ObterQuantidadeDeEventosPorMeses(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, Usuario usuario, Guid usuarioPerfil,
            bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme);

        Task<bool> TemEventoNosDiasETipo(DateTime dataInicio, DateTime dataFim, TipoEvento tipoEventoCodigo, long tipoCalendarioId, string UeId, string DreId, bool escopoRetroativo = false);
               
        Task<IEnumerable<Evento>> ObterEventosCalendarioProfessorPorMesDia(long tipoCalendarioId, string dreCodigo, string ueCodigo, DateTime dataEvento, bool VisualizarEventosSME = false, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = false);

        Task<bool> DataPossuiEventoLiberacaoExcepcionalAsync(long tipoCalendarioId, DateTime dataAula, string ueId);
    }
}