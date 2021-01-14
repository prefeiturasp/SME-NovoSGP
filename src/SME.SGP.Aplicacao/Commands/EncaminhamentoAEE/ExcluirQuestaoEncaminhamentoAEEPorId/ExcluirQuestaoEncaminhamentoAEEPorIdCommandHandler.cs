using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoEncaminhamentoAEEPorIdCommandHandler : IRequestHandler<ExcluirQuestaoEncaminhamentoAEEPorIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamentoAEE { get; }

        public ExcluirQuestaoEncaminhamentoAEEPorIdCommandHandler(IMediator mediator, IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoEncaminhamentoAEE = repositorioQuestaoEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoAEE));
        }

        public async Task<bool> Handle(ExcluirQuestaoEncaminhamentoAEEPorIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestaoEncaminhamentoAEE.RemoverLogico(request.QuestaoId);

            var questoesIds = await repositorioQuestaoEncaminhamentoAEE.ObterQuestoesPorSecaoId(request.QuestaoId);

            foreach (var questaoId in questoesIds)
                await mediator.Send(new ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommand(questaoId));

            return true;
        }
    }
}
