using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesItineranciaPorIdQueryHandler : IRequestHandler<ObterQuestoesItineranciaPorIdQuery, IEnumerable<ItineranciaQuestaoDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterQuestoesItineranciaPorIdQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<IEnumerable<ItineranciaQuestaoDto>> Handle(ObterQuestoesItineranciaPorIdQuery request, CancellationToken cancellationToken)
                => await repositorioItinerancia.ObterQuestoesItineranciaPorId(request.Id, (long)TipoQuestionario.RegistroItinerancia);
    }
}
