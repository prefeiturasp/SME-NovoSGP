using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommandHandler : IRequestHandler<ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE { get; }

        public ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommandHandler(IMediator mediator, IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoAEE = repositorioRespostaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioRespostaEncaminhamentoAEE));
        }

        public async Task<bool> Handle(ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommand request, CancellationToken cancellationToken)
        {
            var respostas = await repositorioRespostaEncaminhamentoAEE.ObterPorQuestaoEncaminhamentoId(request.QuestaoEncaminhamentoAEEId);

            foreach(var resposta in respostas)
                await mediator.Send(new ExcluirRespostaEncaminhamentoAEECommand(resposta));

            return true;
        }
    }
}
