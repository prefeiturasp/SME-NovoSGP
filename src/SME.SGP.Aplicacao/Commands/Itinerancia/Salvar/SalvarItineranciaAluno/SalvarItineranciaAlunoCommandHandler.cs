using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaAlunoCommandHandler : IRequestHandler<SalvarItineranciaAlunoCommand, AuditoriaDto>
    {
        private readonly IRepositorioItineranciaAluno repositorioItineranciaAluno;
        private readonly IMediator mediator;

        public SalvarItineranciaAlunoCommandHandler(IRepositorioItineranciaAluno repositorioItineranciaAluno, IMediator mediator)
        {
            this.repositorioItineranciaAluno = repositorioItineranciaAluno ?? throw new ArgumentNullException(nameof(repositorioItineranciaAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Handle(SalvarItineranciaAlunoCommand request, CancellationToken cancellationToken)
        {
            var itineranciaAluno = MapearParaEntidade(request);

            var itineranciaAlunoId = await repositorioItineranciaAluno.SalvarAsync(itineranciaAluno);

            if (itineranciaAlunoId < 0)
                throw new NegocioException($"Não foi possível salvar a itinerância do aluno");

            if (request.Aluno.Questoes == null || request.Aluno.Questoes.Any())
                    foreach (var questão in request.Aluno.Questoes)
                        await mediator.Send(new SalvarItineranciaAlunoQuestaoCommand(questão.QuestaoId, itineranciaAlunoId, questão.Resposta));

            return (AuditoriaDto)itineranciaAluno;
        }

        private ItineranciaAluno MapearParaEntidade(SalvarItineranciaAlunoCommand request)
            => new ItineranciaAluno()
            {
                CodigoAluno = request.Aluno.CodigoAluno,
                ItineranciaId = request.ItineranciaId
            };
    }
}
