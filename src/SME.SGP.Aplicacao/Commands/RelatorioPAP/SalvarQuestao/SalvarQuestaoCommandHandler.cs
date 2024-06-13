using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarQuestaoCommandHandler : IRequestHandler<SalvarQuestaoCommand, bool>
    {
        private readonly IMediator mediator;

        public SalvarQuestaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(SalvarQuestaoCommand request, CancellationToken cancellationToken)
        {
            var relatorioQuestao = await mediator.Send(new SalvarRelatorioPeriodicoQuestaoPAPCommand(request.RelatorioSecaoId, request.QuestaoId), cancellationToken);
            foreach (var resposta in request.Respostas)
                await mediator.Send(new SalvarRelatorioPeriodicoRespostaPAPCommand(resposta.Resposta, resposta.TipoQuestao, relatorioQuestao.Id), cancellationToken);
            return true;
        }
    }
}