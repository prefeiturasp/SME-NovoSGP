using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTodosUesIdsComPendenciaCalendarioQueryHandler  : IRequestHandler<ObterTodosUesIdsComPendenciaCalendarioQuery, IEnumerable<TodosUesIdsComPendenciaCalendarioDto>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterTodosUesIdsComPendenciaCalendarioQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public Task<IEnumerable<TodosUesIdsComPendenciaCalendarioDto>> Handle(ObterTodosUesIdsComPendenciaCalendarioQuery request, CancellationToken cancellationToken)
        {
            return repositorioUe.ObterTodosUesIdComPendenciasCalendario(request.AnoLetivo);
        }
    }
}