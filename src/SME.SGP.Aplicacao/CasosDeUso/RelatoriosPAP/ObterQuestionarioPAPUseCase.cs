using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioPAPUseCase : IObterQuestionarioPAPUseCase
    {
        private readonly IMediator mediator;

        public ObterQuestionarioPAPUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task<IEnumerable<QuestaoDto>> Executar(long questionarioId, long? papSecaoId)
        {
            return this.mediator.Send(new ObterQuestionarioPAPQuery() { QuestionarioId = questionarioId, PAPSecaoId = papSecaoId });
        }
    }
}
