using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaEncaminhamentoNAAPAPorQuestaoIdCommandHandler : IRequestHandler<ExcluirRespostaEncaminhamentoNAAPAPorQuestaoIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioRespostaEncaminhamentoNAAPA repositorioRespostaEncaminhamentoNAAPA { get; }

        public ExcluirRespostaEncaminhamentoNAAPAPorQuestaoIdCommandHandler(IMediator mediator, IRepositorioRespostaEncaminhamentoNAAPA repositorioRespostaEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoNAAPA = repositorioRespostaEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioRespostaEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExcluirRespostaEncaminhamentoNAAPAPorQuestaoIdCommand request, CancellationToken cancellationToken)
        {
            var respostas = await repositorioRespostaEncaminhamentoNAAPA.ObterPorQuestaoEncaminhamentoId(request.QuestaoEncaminhamentoNAAPAId);

            foreach(var resposta in respostas)
                await mediator.Send(new ExcluirRespostaEncaminhamentoNAAPACommand(resposta));

            return true;
        }
    }
}
