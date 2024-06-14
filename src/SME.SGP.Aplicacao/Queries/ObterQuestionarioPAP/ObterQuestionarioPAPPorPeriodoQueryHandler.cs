using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioPAPPorPeriodoQueryHandler : IRequestHandler<ObterQuestionarioPAPPorPeriodoQuery,IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;

        public ObterQuestionarioPAPPorPeriodoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestionarioPAPPorPeriodoQuery request, CancellationToken cancellationToken)
        {
            var periodoPAP = await mediator.Send(new PeriodoRelatorioPAPQuery(request.PeriodoIdPAP), cancellationToken);

            return await mediator.Send(new ObterQuestionarioPAPQuery(request.CodigoTurma, request.CodigoAluno, periodoPAP, request.QuestionarioId, request.PapSecaoId), cancellationToken);
        }
    }
}