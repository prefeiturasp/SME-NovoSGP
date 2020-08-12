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
                    Data = c,
                    EhLetivo = !c.FimDeSemana()
                }));
            }

            var eventos = (await repositorioEvento.ObterEventosPorTipoDeCalendarioAsync(request.TipoCalendarioId))?.Where(c => (c.EhEventoUE() || c.EhEventoSME()));

            if (eventos != null)
            {
                var eventosNoMesmoDia = eventos.Where(c => c.DataInicio == c.DataFim);
                var eventosComRangeDeDatas = eventos.Where(c => c.DataInicio != c.DataFim);

                var datasComEventos = eventos.SelectMany(e => e.ObterIntervaloDatas().Select(c => new DiaLetivoDto
                {
                    Data = c,
                    EhLetivo = e.EhEventoLetivo() || (!e.EhEventoLetivo() && !e.DataInicio.FimDeSemana()),
                    UesIds = string.IsNullOrWhiteSpace(e.UeId) ? new List<string>() : new List<string> { e.UeId }
                }));

                datasDosPeriodosEscolares.AddRange(datasComEventos);

                foreach (var dia in datasDosPeriodosEscolares.OrderBy(c => c.Data))
                {
                    var eventosNoDia = eventos.Where(c => c.DataEstaNoRangeDoEvento(dia.Data) && (dia.UesIds.Any() ? dia.UesIds.Contains(c.UeId) : string.IsNullOrWhiteSpace(c.UeId)));


                    var temEventoLetivo = eventosNoDia.Any(c => c.EhEventoLetivo());
                    var temEventoNaoLetivo = eventosNoDia.Any(c => c.NaoEhEventoLetivo());
                    var ehEventoSme = eventosNoDia.Any(c => c.EhEventoSME());

                    dia.EhLetivo = eventosNoDia == null ? !dia.Data.FimDeSemana() : (temEventoLetivo || (!temEventoNaoLetivo && !dia.Data.FimDeSemana()));
                    dia.UesIds = ehEventoSme ? new List<string>() : eventosNoDia?.Select(c => c.UeId)?.ToList();
                }
            }
            return datasDosPeriodosEscolares;
        }
    }
}