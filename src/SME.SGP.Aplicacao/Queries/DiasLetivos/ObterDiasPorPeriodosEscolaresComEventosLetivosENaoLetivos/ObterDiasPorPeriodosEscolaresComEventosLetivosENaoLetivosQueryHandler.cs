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
            foreach (var periodoEscolar in request.PeriodosEscolares.OrderBy(c=>c.Bimestre))
            {
                datasDosPeriodosEscolares.AddRange(periodoEscolar.ObterIntervaloDatas().Select(c => new DiaLetivoDto
                {
                    Data = c,
                    EhLetivo = !c.FimDeSemana()
                }));
            }

            var eventos = (await repositorioEvento.ObterEventosPorTipoDeCalendarioAsync(request.TipoCalendarioId))?.Where(c => (c.EhEventoUE() || c.EhEventoSME()));
            if (eventos != null)
            {
                datasDosPeriodosEscolares.AddRange(ObtemEventoLetivosFimDeSemana(eventos));

                datasDosPeriodosEscolares.AddRange(ObtemEventosNaoLetivos(datasDosPeriodosEscolares, eventos));
            }

            return datasDosPeriodosEscolares;
        }

        private IEnumerable<DiaLetivoDto> ObtemEventosNaoLetivos(List<DiaLetivoDto> datasDosPeriodosEscolares, IEnumerable<Evento> eventos)
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
                }));;

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
    }
}