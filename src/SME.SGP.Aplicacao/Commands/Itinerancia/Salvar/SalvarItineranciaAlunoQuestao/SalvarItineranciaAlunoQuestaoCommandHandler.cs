using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaAlunoQuestaoCommandHandler : IRequestHandler<SalvarItineranciaAlunoQuestaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioItineranciaAlunoQuestao repositorioItineranciaAlunoQuestao;

        public SalvarItineranciaAlunoQuestaoCommandHandler(IRepositorioItineranciaAlunoQuestao repositorioItineranciaAlunoQuestao)
        {
            this.repositorioItineranciaAlunoQuestao = repositorioItineranciaAlunoQuestao ?? throw new ArgumentNullException(nameof(repositorioItineranciaAlunoQuestao));
        }

        public async Task<AuditoriaDto> Handle(SalvarItineranciaAlunoQuestaoCommand request, CancellationToken cancellationToken)
        {
            var itineranciaAlunoQuestao = MapearParaEntidade(request);

            await repositorioItineranciaAlunoQuestao.SalvarAsync(itineranciaAlunoQuestao);

            return (AuditoriaDto)itineranciaAlunoQuestao;
        }
        private ItineranciaAlunoQuestao MapearParaEntidade(SalvarItineranciaAlunoQuestaoCommand request)
            => new ItineranciaAlunoQuestao()
            {
                QuestaoId = request.QuestaoId,
                ItineranciaAlunoId = request.ItineranciaAlunoId,
                Resposta = request.Resposta
            };
    }
}
