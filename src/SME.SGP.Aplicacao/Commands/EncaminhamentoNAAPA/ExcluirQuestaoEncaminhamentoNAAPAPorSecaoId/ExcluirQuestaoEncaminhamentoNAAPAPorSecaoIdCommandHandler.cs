using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommandHandler : IRequestHandler<ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA { get; }

        public ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommandHandler(IMediator mediator, IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommand request, CancellationToken cancellationToken)
        {
            var questoesIds = await repositorioQuestaoEncaminhamentoNAAPA.ObterQuestoesPorSecaoId(request.EncaminhamentoNAAPASecaoId);

            foreach (var questaoId in questoesIds)
                await mediator.Send(new ExcluirRespostaEncaminhamentoNAAPAPorQuestaoIdCommand(questaoId));

            await repositorioQuestaoEncaminhamentoNAAPA.RemoverLogico(request.EncaminhamentoNAAPASecaoId, "encaminhamento_naapa_secao_id");

            return true;
        }
    }
}
