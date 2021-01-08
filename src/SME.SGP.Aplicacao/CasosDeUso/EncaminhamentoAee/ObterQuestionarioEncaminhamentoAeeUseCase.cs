using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioEncaminhamentoAeeUseCase : IObterQuestionarioEncaminhamentoAeeUseCase
    {
        private readonly IMediator mediator;

        public ObterQuestionarioEncaminhamentoAeeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<QuestaoAeeDto>> Executar(long questionarioId, long? encaminhamentoId)
        {
            return 
                await mediator
                .Send(new ObterQuestionarioEncaminhamentoAeeQuery(questionarioId, encaminhamentoId));
        }
    }
}
