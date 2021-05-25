using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesPlanoAEEPorVersaoQueryHandler : IRequestHandler<ObterQuestoesPlanoAEEPorVersaoQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;

        public ObterQuestoesPlanoAEEPorVersaoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestoesPlanoAEEPorVersaoQuery request, CancellationToken cancellationToken)
        {
            var respostasPlano = request.VersaoPlanoId > 0 ?
                await mediator.Send(new ObterRespostasPlanoAEEPorVersaoQuery(request.VersaoPlanoId)) :
                Enumerable.Empty<RespostaQuestaoDto>();

            return await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.QuestionarioId, questaoId =>
               respostasPlano.Where(c => c.QuestaoId == questaoId)));
        }
    }
}
