using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardItineranciaObjetivosQueryHandler : IRequestHandler<ObterDashboardItineranciaObjetivosQuery, IEnumerable<DashboardItineranciaDto>>
    {
        private readonly IRepositorioItinerancia repositorio;

        public ObterDashboardItineranciaObjetivosQueryHandler(IRepositorioItinerancia repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<DashboardItineranciaDto>> Handle(ObterDashboardItineranciaObjetivosQuery request, CancellationToken cancellationToken)
         => await repositorio.ObterQuantidadeObjetivos(request.Ano, request.DreId, request.UeId, request.Mes, request.CodigoRF);
    }
}
