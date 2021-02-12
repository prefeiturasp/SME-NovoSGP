using MediatR;
using SME.SGP.Dominio;
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
            var id = await repositorioItineranciaAluno.RemoverLogico(request.Id);

            if (id == 0)
                throw new NegocioException($"Não foi possível excluir o aluno da itinerância do id {id}");


            var questoesAluno = await mediator.Send(new ObterQuestoesItineranciaAlunoPorIdQuery(id));
            if (questoesAluno == null || !questoesAluno.Any())
                throw new NegocioException($"Não foi possível obter as questoes do aluno para o id {id}");

            foreach (var questao in questoesAluno)
                await mediator.Send(new ExcluirItineranciaAlunoQuestaoCommand(questao.Id));


            return id != 0;
        }
    }
}
