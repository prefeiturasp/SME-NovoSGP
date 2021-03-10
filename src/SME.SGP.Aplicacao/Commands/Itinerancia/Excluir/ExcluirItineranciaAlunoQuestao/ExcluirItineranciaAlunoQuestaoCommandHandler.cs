using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaAlunoQuestaoCommandHandler : IRequestHandler<ExcluirItineranciaAlunoQuestaoCommand, bool>
    {
        private readonly IRepositorioItineranciaAlunoQuestao repositorioItineranciaAlunoQuestao;

        public ExcluirItineranciaAlunoQuestaoCommandHandler(IRepositorioItineranciaAlunoQuestao repositorioItineranciaAlunoQuestao)
        {
            this.repositorioItineranciaAlunoQuestao = repositorioItineranciaAlunoQuestao ?? throw new ArgumentNullException(nameof(repositorioItineranciaAlunoQuestao));
        }

        public async Task<bool> Handle(ExcluirItineranciaAlunoQuestaoCommand request, CancellationToken cancellationToken)
        {
            repositorioItineranciaAlunoQuestao.Remover(request.Id);

            return true;
        }
    }
}
