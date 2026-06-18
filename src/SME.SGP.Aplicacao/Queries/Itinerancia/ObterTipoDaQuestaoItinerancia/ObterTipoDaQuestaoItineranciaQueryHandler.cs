using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDaQuestaoItineranciaQueryHandler : IRequestHandler<ObterTipoDaQuestaoItineranciaQuery, List<QuestaoTipoDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterTipoDaQuestaoItineranciaQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<List<QuestaoTipoDto>> Handle(ObterTipoDaQuestaoItineranciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioItinerancia.ObterTipoDaQuestaoItinerancia(request.ItineranciaId);
        }
    }

}