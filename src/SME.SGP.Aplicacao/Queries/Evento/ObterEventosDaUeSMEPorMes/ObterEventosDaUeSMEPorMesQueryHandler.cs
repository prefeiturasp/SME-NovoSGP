using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosDaUeSMEPorMesQueryHandler : IRequestHandler<ObterEventosDaUeSMEPorMesQuery, IEnumerable<Evento>>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterEventosDaUeSMEPorMesQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }
        public async Task<IEnumerable<Evento>> Handle(ObterEventosDaUeSMEPorMesQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEvento.ObterEventosCalendarioProfessorPorMes(request.TipoCalendarioId, request.DreCodigo, request.UeCodigo, request.Mes, true);
        }
    }
}
