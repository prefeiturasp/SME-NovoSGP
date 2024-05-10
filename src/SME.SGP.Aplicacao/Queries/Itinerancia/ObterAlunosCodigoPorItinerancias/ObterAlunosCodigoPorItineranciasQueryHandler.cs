using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosCodigoPorItineranciasQueryHandler : IRequestHandler<ObterAlunosCodigoPorItineranciasQuery, IEnumerable<ItineranciaCodigoAlunoDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterAlunosCodigoPorItineranciasQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<IEnumerable<ItineranciaCodigoAlunoDto>> Handle(ObterAlunosCodigoPorItineranciasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioItinerancia.ObterCodigoAlunosPorItineranciasIds(request.ItineranciasIds);
        }
    }
}
