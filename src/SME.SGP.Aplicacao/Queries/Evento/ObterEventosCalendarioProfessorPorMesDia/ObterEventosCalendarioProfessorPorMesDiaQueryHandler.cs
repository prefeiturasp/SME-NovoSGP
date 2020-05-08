using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosCalendarioProfessorPorMesDiaQueryHandler : IRequestHandler<ObterEventosCalendarioProfessorPorMesDiaQuery, IEnumerable<Evento>>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterEventosCalendarioProfessorPorMesDiaQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }
        public async Task<IEnumerable<Evento>> Handle(ObterEventosCalendarioProfessorPorMesDiaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEvento.ObterEventosCalendarioProfessorPorMesDia(request.TipoCalendarioId, request.DreCodigo, request.UeCodigo, request.DataConsulta, true);
        }
    }
}
