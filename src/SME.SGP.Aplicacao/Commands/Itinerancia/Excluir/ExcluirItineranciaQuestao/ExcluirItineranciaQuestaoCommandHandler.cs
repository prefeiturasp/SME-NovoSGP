using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaQuestaoCommandHandler : IRequestHandler<ExcluirItineranciaQuestaoCommand, bool>
    {
        private readonly IRepositorioItineranciaQuestao repositorioItineranciaQuestao;

        public ExcluirItineranciaQuestaoCommandHandler(IRepositorioItineranciaQuestao repositorioItineranciaQuestao)
        {
            this.repositorioItineranciaQuestao = repositorioItineranciaQuestao ?? throw new ArgumentNullException(nameof(repositorioItineranciaQuestao));
        }

        public async Task<bool> Handle(ExcluirItineranciaQuestaoCommand request, CancellationToken cancellationToken)
        {
            await repositorioItineranciaQuestao.ExcluirItineranciaQuestao(request.QuestaoId, request.ItineranciaId);
            return true;
        }
    }
}
