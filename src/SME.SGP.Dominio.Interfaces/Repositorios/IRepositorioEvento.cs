﻿using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEvento : IRepositorioBase<Evento>
    {
        Task<IEnumerable<Evento>> ObterEventosCalendarioProfessorPorMes(long tipoCalendarioId, string dreCodigo, string ueCodigo, int mes, bool VisualizarEventosSME = false, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = false);
        List<Evento> EhEventoLetivoPorLiberacaoExcepcional(long tipoCalendarioId, DateTime dataAula, string ueId);

        Task<bool> EhEventoLetivoPorTipoDeCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId);
        Task<bool> EhEventoNaoLetivoPorTipoDeCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId);

        Task<long> ObterTipoCalendarioIdPorEvento(long eventoId);
        Task<IEnumerable<Evento>> ObterEventosPorTipoEData(TipoEvento tipoEvento, DateTime data);
        Task<IEnumerable<EventoDataDto>> ListarEventosItinerancia(long tipoCalendarioId, long ItineranciaId, string codigoUE, string login, Guid perfil, bool historico = false);
        Task<IEnumerable<Evento>> ObterEventosPorTipoECalendarioUe(long tipoCalendarioId, string ueCodigo, TipoEvento tipoEvento);
        Task<IEnumerable<Evento>> EventosNosDiasETipo(DateTime dataInicio, DateTime dataFim, TipoEvento tipoEventoCodigo, long tipoCalendarioId, string UeId, string DreId, bool utilizarRangeDatas = true);
        bool ExisteEventoPorEventoTipoId(long eventoTipoId);

        bool ExisteEventoPorFeriadoId(long feriadoId);

        bool ExisteEventoPorTipoCalendarioId(long tipoCalendarioId);
        Task<PaginacaoResultadoDto<Evento>> Listar(long? tipoCalendarioId, long? tipoEventoId, string nomeEvento, DateTime? dataInicio, DateTime? dataFim, Paginacao paginacao, string dreId, string ueId,
           bool ehTodasDres, bool ehTodasUes, Usuario usuario, Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor, bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, bool consideraHistorico, bool eventosTodaRede);

        Task<IEnumerable<CalendarioEventosNoDiaRetornoDto>> ObterEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes, int dia, int anoLetivo, Usuario usuario);

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

        Task<IEnumerable<Evento>> ObterEventosCalendarioProfessorPorMesDia(string dreCodigo, string ueCodigo, DateTime dataEvento, bool VisualizarEventosSME = false, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = false);

        Task<bool> DataPossuiEventoLiberacaoExcepcionalAsync(long tipoCalendarioId, DateTime dataAula, string ueId);
        Task<IEnumerable<Evento>> ObterEventosPorTipoDeCalendarioAsync(long tipoCalendarioId, params EventoLetivo[] tiposLetivosConsiderados);
        Task<IEnumerable<EventoCalendarioRetornoDto>> ObterEventosPorTipoDeCalendarioDreUeModalidadeAsync(long tipoCalendario, int anoLetivo, string codigoDre, string codigoUe, int? modalidade);
        Task<IEnumerable<Evento>> ObterEventosPorTipoCalendarioIdEPeriodoInicioEFim(long tipoCalendarioId, DateTime periodoInicio, DateTime periodoFim, long? turmaId = null);
        Task<Evento> ObterEventoAtivoPorId(long eventoId);
        Task<IEnumerable<EventoCalendarioRetornoDto>> ObterEventosPorTipoDeCalendarioDreUeEModalidades(long tipoCalendario, int anoLetivo, string dreCodigo, string ueCodigo, IEnumerable<int> modalidades);
    }
}