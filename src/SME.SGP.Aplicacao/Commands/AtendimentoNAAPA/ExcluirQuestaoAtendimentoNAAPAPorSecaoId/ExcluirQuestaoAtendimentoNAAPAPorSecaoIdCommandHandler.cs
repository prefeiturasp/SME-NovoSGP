using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoAtendimentoNAAPAPorSecaoIdCommandHandler : IRequestHandler<ExcluirQuestaoAtendimentoNAAPAPorSecaoIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamentoNAAPA { get; }

        public ExcluirQuestaoAtendimentoNAAPAPorSecaoIdCommandHandler(IMediator mediator, IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExcluirQuestaoAtendimentoNAAPAPorSecaoIdCommand request, CancellationToken cancellationToken)
        {
            var questoesIds = await repositorioQuestaoEncaminhamentoNAAPA.ObterQuestoesPorSecaoId(request.EncaminhamentoNAAPASecaoId);

            foreach (var questaoId in questoesIds)
                await mediator.Send(new ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommand(questaoId));

            await repositorioQuestaoEncaminhamentoNAAPA.RemoverLogico(request.EncaminhamentoNAAPASecaoId, "encaminhamento_naapa_secao_id");

            return true;
        }
    }
}
