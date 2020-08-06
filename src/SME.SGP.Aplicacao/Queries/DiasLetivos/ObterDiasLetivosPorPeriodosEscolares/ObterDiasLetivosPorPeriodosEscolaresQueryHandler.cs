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
    public class ObterDiasLetivosPorPeriodosEscolaresQueryHandler : IRequestHandler<ObterDiasLetivosPorPeriodosEscolaresQuery, List<DiaLetivoDto>>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterDiasLetivosPorPeriodosEscolaresQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }
        public async Task<List<DiaLetivoDto>> Handle(ObterDiasLetivosPorPeriodosEscolaresQuery request, CancellationToken cancellationToken)
        {
            var datasDosPeriodosEscolares = new List<DiaLetivoDto>();
            foreach (var periodoEscolar in request.PeriodosEscolares)
            {
                datasDosPeriodosEscolares.AddRange(periodoEscolar.ObterIntervaloDatas().Select(c => new DiaLetivoDto
                {
                    Data = c
                }));
            }

            var eventos = await repositorioEvento.ObterEventosPorTipoDeCalendarioAsync(request.TipoCalendarioId);

            foreach (var dia in datasDosPeriodosEscolares)
            {
                var eventosNoDia = eventos.Where(c => c.DataEstaNoRangeDoEvento(dia.Data));
                var temEventoLetivo = eventosNoDia.Any(c => c.Letivo == EventoLetivo.Sim);
                var temEventoNaoLetivo = eventosNoDia.Any(c => c.Letivo == EventoLetivo.Nao);

                dia.EhLetivo = eventosNoDia == null ? !dia.Data.FimDeSemana() : (temEventoLetivo || (!temEventoNaoLetivo && !dia.Data.FimDeSemana()));
            }

            return datasDosPeriodosEscolares;
        }
    }
}