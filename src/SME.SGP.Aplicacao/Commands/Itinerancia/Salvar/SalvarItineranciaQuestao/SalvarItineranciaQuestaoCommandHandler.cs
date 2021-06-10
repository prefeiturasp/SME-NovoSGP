using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaQuestaoCommandHandler : IRequestHandler<SalvarItineranciaQuestaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioItineranciaQuestao repositorioItineranciaQuestao;

        public SalvarItineranciaQuestaoCommandHandler(IRepositorioItineranciaQuestao repositorioItineranciaQuestao)
        {
            this.repositorioItineranciaQuestao = repositorioItineranciaQuestao ?? throw new ArgumentNullException(nameof(repositorioItineranciaQuestao));
        }

        public async Task<AuditoriaDto> Handle(SalvarItineranciaQuestaoCommand request, CancellationToken cancellationToken)
        {
            var itineranciaQuestao = MapearParaEntidade(request);

            await repositorioItineranciaQuestao.SalvarAsync(itineranciaQuestao);

            return (AuditoriaDto)itineranciaQuestao;
        }
        private ItineranciaQuestao MapearParaEntidade(SalvarItineranciaQuestaoCommand request)
            => new ItineranciaQuestao()
            {
                QuestaoId = request.QuestaoId,
                ItineranciaId = request.ItineranciaId,
                Resposta = request.Resposta
            };
    }
}
