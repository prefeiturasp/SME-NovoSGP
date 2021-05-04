using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaAlunoCommandHandler : IRequestHandler<ExcluirItineranciaAlunoCommand, bool>
    {
        private readonly IRepositorioItineranciaAluno repositorioItineranciaAluno;
        private readonly IMediator mediator;

        public ExcluirItineranciaAlunoCommandHandler(IRepositorioItineranciaAluno repositorioItineranciaAluno, IMediator mediator)
        {
            this.repositorioItineranciaAluno = repositorioItineranciaAluno ?? throw new ArgumentNullException(nameof(repositorioItineranciaAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirItineranciaAlunoCommand request, CancellationToken cancellationToken)
        {
            if (request.ItineranciaAluno.AlunosQuestoes == null || request.ItineranciaAluno.AlunosQuestoes.Any())
                foreach (var questao in request.ItineranciaAluno.AlunosQuestoes)
                    await mediator.Send(new ExcluirItineranciaAlunoQuestaoCommand(questao.Id));

            repositorioItineranciaAluno.Remover(request.ItineranciaAluno.Id);

            return true;
        }
    }
}
