using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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

        public ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<List<DiaLetivoDto>> Handle(ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery request, CancellationToken cancellationToken)
        {
            var datasDosPeriodosEscolares = new List<DiaLetivoDto>();
            var eventos = (await repositorioEvento.ObterEventosPorTipoDeCalendarioAsync(request.TipoCalendarioId))?.Where(c => (c.EhEventoUE() || c.EhEventoSME()));
            var periodosEventosFeriados = eventos?.Where(e => e.EhFeriado()).Select(e => (e.DataInicio.Date, e.DataFim));

            foreach (var periodoEscolar in request.PeriodosEscolares.OrderBy(c => c.Bimestre))
            {
                datasDosPeriodosEscolares.AddRange(periodoEscolar.ObterIntervaloDatas(periodosEventosFeriados).Select(c => new DiaLetivoDto
                {
                    Data = c,
                    EhLetivo = eventos.Any(e => e.Letivo == EventoLetivo.Sim && c.Date >= e.DataInicio.Date && c.Date <= e.DataFim.Date) ||
                               (!c.FimDeSemana() && !eventos.Any(e => e.Letivo == EventoLetivo.Nao && c.Date >= e.DataInicio.Date && c.Date <= e.DataFim.Date)),
                    DreIds = ObterUnidades(eventos, c.Date, true).ToList(),
                    UesIds = ObterUnidades(eventos, c.Date, false).ToList()
                }));
            }

            if (eventos != null)
            {
                datasDosPeriodosEscolares.AddRange(ObtemEventoLetivosFimDeSemana(eventos));

                datasDosPeriodosEscolares.AddRange(ObtemEventosNaoLetivos(eventos));
            }

            return datasDosPeriodosEscolares;
        }

        private IEnumerable<DiaLetivoDto> ObtemEventosNaoLetivos(IEnumerable<Evento> eventos)
        {
            var datasComEventosNaoLetivos = eventos.Where(c => c.EhEventoNaoLetivo())
                .SelectMany(evento => evento.ObterIntervaloDatas()
                .Where(c => !c.FimDeSemana())
                .Select(data => new DiaLetivoDto
                {
                    Data = data,
                    EhLetivo = false,
                    EhNaoLetivo = true,
                    Motivo = evento.Nome,
                    UesIds = string.IsNullOrWhiteSpace(evento.UeId) ? new List<string>() : new List<string> { evento.UeId },
                    DreIds = string.IsNullOrWhiteSpace(evento.DreId) ? new List<string>() : new List<string> { evento.DreId },
                    PossuiEvento = true
                }));

            return datasComEventosNaoLetivos;
        }

        private IEnumerable<DiaLetivoDto> ObtemEventoLetivosFimDeSemana(IEnumerable<Evento> eventos)
        {
            var datasComEventosFimDeSemana = eventos.Where(c => c.EhEventoLetivo())
                .SelectMany(evento => evento.ObterIntervaloDatas()
                .Where(c => c.FimDeSemana())
                .Select(data => new DiaLetivoDto
                {
                    Data = data,
                    EhLetivo = true,
                    UesIds = string.IsNullOrWhiteSpace(evento.UeId) ? new List<string>() : new List<string> { evento.UeId },
                    DreIds = string.IsNullOrWhiteSpace(evento.DreId) ? new List<string>() : new List<string> { evento.DreId },
                    PossuiEvento = true
                }));

            return datasComEventosFimDeSemana;
        }

        private IList<string> ObterUnidades(IEnumerable<Evento> eventos, DateTime dataConsiderada, bool obterDres)
        {
            var listaRetorno = new List<string>();            

            var eventosPorData = eventos?
                .Where(e => dataConsiderada >= e.DataInicio.Date && dataConsiderada <= e.DataFim.Date && (obterDres ? e.EhEventoDRE() : e.EhEventoUE()));

            if (eventosPorData != null && eventosPorData.Any())
            {
                listaRetorno.AddRange((from e in eventosPorData
                                       select (obterDres ? e.DreId : e.UeId)).ToList());
            }

            return listaRetorno;
        }
    }
}