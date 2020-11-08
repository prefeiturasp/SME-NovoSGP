using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosPorTipoCalendarioIdTurmaIdEDataQueryHandler : IRequestHandler<ObterEventosPorTipoCalendarioIdTurmaIdEDataQuery, IEnumerable<Evento>>
    {
        private readonly IRepositorioEvento repositorioEvento;
        public ObterEventosPorTipoCalendarioIdTurmaIdEDataQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public Task<IEnumerable<Evento>> Handle(ObterEventosPorTipoCalendarioIdTurmaIdEDataQuery request, CancellationToken cancellationToken)
                => repositorioEvento.ObterEventosPorTipoCalendarioIdEPeriodoInicioEFim(request.TipoCalendarioId, request.PeriodoInicio, request.PeriodoFim, request.TurmaId);
    }
}
