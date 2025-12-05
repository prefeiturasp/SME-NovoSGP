using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoAtendimentoNAAPAPorIdCommandHandler : IRequestHandler<ExcluirQuestaoAtendimentoNAAPAPorIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamentoNAAPA { get; }

        public ExcluirQuestaoAtendimentoNAAPAPorIdCommandHandler(IMediator mediator, IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExcluirQuestaoAtendimentoNAAPAPorIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestaoEncaminhamentoNAAPA.RemoverLogico(request.QuestaoId);
            await mediator.Send(new ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommand(request.QuestaoId));

            return true;
        }
    }
}
