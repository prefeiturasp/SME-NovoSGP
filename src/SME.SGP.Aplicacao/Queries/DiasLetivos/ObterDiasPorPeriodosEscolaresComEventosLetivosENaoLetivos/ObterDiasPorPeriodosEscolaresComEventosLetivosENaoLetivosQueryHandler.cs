using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQueryHandler : IRequestHandler<ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery, List<DiaLetivoDto>>
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioCache repositorioCache;

        public ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQueryHandler(IRepositorioEvento repositorioEvento, IRepositorioCache repositorioCache)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<List<DiaLetivoDto>> Handle(ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery request, CancellationToken cancellationToken)
        {
            var chaveCache = string.Format(NomeChaveCache.DIAS_LETIVOS_E_NAO_LETIVOS_PERIODO_ESCOLAR_IDS_CONSIDERADOS_DESCONSIDERA_CRIACAO_DIA_LETIVO_PROXIMAS_UES,
                string.Join(",", request.PeriodosEscolares.Select(p => p.Id)), request.DesconsiderarCriacaoDiaLetivoProximasUes, request.UeCodigo);

            return await repositorioCache.ObterAsync(chaveCache, async
                () => await ProcessarDiasLetivos(request.TipoCalendarioId, request.PeriodosEscolares, request.UeCodigo, request.DesconsiderarCriacaoDiaLetivoProximasUes), minutosParaExpirar: 300);
        }

        private async Task<List<DiaLetivoDto>> ProcessarDiasLetivos(long tipoCalendarioId, IEnumerable<PeriodoEscolar> periodoEscolares, string ueCodigo, bool desconsiderarCriacaoDiaLetivoProximasUes)
        {
            var datasDosPeriodosEscolares = new List<DiaLetivoDto>();

            var tiposLetivosConsiderados = new int[] { (int)EventoLetivo.Sim, (int)EventoLetivo.Nao };

            var chaveCacheEventosTipoCalendario = string.Format(NomeChaveCache.EVENTOS_TIPO_CALENDARIO_UE_TIPOS_LETIVOS_CONSIDERADOS,
                tipoCalendarioId, ueCodigo, string.Join(",", tiposLetivosConsiderados));

            var eventosTipoCalendario = await repositorioCache
                .ObterAsync(chaveCacheEventosTipoCalendario, async
                    () => await repositorioEvento.ObterEventosPorTipoDeCalendarioAsync(tipoCalendarioId, ueCodigo, tiposLetivosConsiderados.Select(tl => (EventoLetivo)tl).ToArray()), minutosParaExpirar: 300);

            var eventos = eventosTipoCalendario?
                .Where(c => c.EhEventoUE() || c.EhEventoSME());

            DefinirDiasLetivos(periodoEscolares, desconsiderarCriacaoDiaLetivoProximasUes, datasDosPeriodosEscolares, eventos);

            return datasDosPeriodosEscolares;
        }

        private static void DefinirDiasLetivos(IEnumerable<PeriodoEscolar> periodosEscolares, bool desconsiderarCriacaoDiaLetivoProximasUes, List<DiaLetivoDto> datasDosPeriodosEscolares, IEnumerable<Evento> eventos)
        {
            foreach (var periodoEscolar in periodosEscolares.OrderBy(c => c.Bimestre))
            {
                foreach (var diaAtual in periodoEscolar.ObterIntervaloDatas())
                {
                    var diaLetivoDto = new DiaLetivoDto()
                    {
                        Data = diaAtual,
                        PossuiEvento = false
                    };

                    var eventosComData = eventos.Where(e => diaAtual.Date >= e.DataInicio.Date 
                                                            && diaAtual.Date <= e.DataFim.Date)
                                                .ToList();
                    PreencherInformacoesDiaLetivo(diaLetivoDto, eventosComData, datasDosPeriodosEscolares, desconsiderarCriacaoDiaLetivoProximasUes, diaAtual);
                }
            }
        }

        private static void PreencherInformacoesDiaLetivo(DiaLetivoDto diaLetivoDto, List<Evento> eventosComData, 
                                             List<DiaLetivoDto> datasDosPeriodosEscolares,
                                             bool desconsiderarCriacaoDiaLetivoProximasUes,
                                             DateTime diaPeriodo)
        {
            if (!eventosComData.Any())
            {
                diaLetivoDto.EhLetivo = !diaPeriodo.FimDeSemana();
                datasDosPeriodosEscolares.Add(diaLetivoDto);
                return;
            }

            var eventosSME = eventosComData.Where(e => e.EhEventoSME());
            if (eventosSME.Any())
            {
                diaLetivoDto.EhLetivo = eventosSME.Any(e => e.EhEventoLetivo());
                diaLetivoDto.Motivo = eventosSME.First().Nome;
                diaLetivoDto.PossuiEvento = true;
                datasDosPeriodosEscolares.Add(diaLetivoDto);
                return;
            }

            var eventosDRE = eventosComData.Where(e => e.EhEventoDRE());
            if (eventosDRE.Any())
            {
                diaLetivoDto.EhLetivo = eventosDRE.Any(e => e.EhEventoLetivo());
                diaLetivoDto.Motivo = eventosDRE.First().Nome;
                diaLetivoDto.DreIds = eventosDRE.Select(e => e.DreId).ToList();
                diaLetivoDto.PossuiEvento = true;
                datasDosPeriodosEscolares.Add(diaLetivoDto);
                return;
            }

            var eventosLetivosNaoLetivosUE = eventosComData.Where(e => e.EhEventoUE());

            if (eventosLetivosNaoLetivosUE.Any())
            {
                eventosLetivosNaoLetivosUE.ToList().ForEach(elue =>
                {
                    datasDosPeriodosEscolares.Add(new DiaLetivoDto()
                    {
                        Data = diaPeriodo,
                        PossuiEvento = true,
                        EhLetivo = EventoEhLetivo(diaPeriodo, elue),
                        Motivo = elue.Nome,
                        UesIds = new List<string>() { elue.UeId }
                    });
                });

                if (desconsiderarCriacaoDiaLetivoProximasUes)
                    return;
            }
            else
            {
                diaLetivoDto.EhLetivo = !diaPeriodo.FimDeSemana();
                datasDosPeriodosEscolares.Add(diaLetivoDto);
            }
        }

        private static bool EventoEhLetivo(DateTime data, Evento evento)
            => (!data.FimDeSemana() && evento.EhEventoLetivo()) ||
               (data.FimDeSemana() && (evento.EhEventoLetivo() || (EventoTipoEnum)evento.TipoEventoId == EventoTipoEnum.ReposicaoAula));
    }
}