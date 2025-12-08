using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommandHandler : IRequestHandler<ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioRespostaAtendimentoNAAPA repositorioRespostaEncaminhamentoNAAPA { get; }

        public ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommandHandler(IMediator mediator, IRepositorioRespostaAtendimentoNAAPA repositorioRespostaEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoNAAPA = repositorioRespostaEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioRespostaEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommand request, CancellationToken cancellationToken)
        {
            var respostas = await repositorioRespostaEncaminhamentoNAAPA.ObterPorQuestaoEncaminhamentoId(request.QuestaoEncaminhamentoNAAPAId);

            foreach(var resposta in respostas)
                await mediator.Send(new ExcluirRespostaAtendimentoNAAPACommand(resposta));

            return true;
        }
    }
}
