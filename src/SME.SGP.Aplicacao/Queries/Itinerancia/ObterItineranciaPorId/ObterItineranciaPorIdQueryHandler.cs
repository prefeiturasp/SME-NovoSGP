using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciaPorIdQueryHandler : IRequestHandler<ObterItineranciaPorIdQuery, ItineranciaDto>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;
        private readonly IMediator mediator;

        public ObterItineranciaPorIdQueryHandler(IRepositorioItinerancia repositorioItinerancia, IMediator mediator)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ItineranciaDto> Handle(ObterItineranciaPorIdQuery request, CancellationToken cancellationToken)
        {
            var itinerancia = await repositorioItinerancia.ObterItineranciaPorId(request.Id);

            if (itinerancia == null)
                throw new NegocioException("Não foi possível obter a itinerancia");

            itinerancia.Alunos = await mediator.Send(new ObterItineranciaAlunoPorIdQuery(request.Id));

            itinerancia.ObjetivosVisita = await mediator.Send(new ObterObjetivosItineranciaPorIdQuery(request.Id));

            itinerancia.Questoes = await mediator.Send(new ObterQuestoesItineranciaPorIdQuery(request.Id));

            itinerancia.Ues = await mediator.Send(new ObterUesItineranciaPorIdQuery(request.Id));

            return itinerancia;
        }
    }
}
