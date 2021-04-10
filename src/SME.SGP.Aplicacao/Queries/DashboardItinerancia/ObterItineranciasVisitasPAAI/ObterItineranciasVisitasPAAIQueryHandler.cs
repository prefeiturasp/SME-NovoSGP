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
    public class ObterItineranciasVisitasPAAIQueryHandler : IRequestHandler<ObterItineranciasVisitasPAAIQuery, IEnumerable<ItineranciaVisitaDto>>
    {
        private readonly IRepositorioItinerancia repositorio;

        public ObterItineranciasVisitasPAAIQueryHandler(IRepositorioItinerancia repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<ItineranciaVisitaDto>> Handle(ObterItineranciasVisitasPAAIQuery request, CancellationToken cancellationToken)
         => await repositorio.ObterQuantidadeVisitasPAAI(request.Ano, request.DreId, request.UeId, request.Mes);
    }
}
