using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioRegistroAcaoUseCase : IObterQuestionarioRegistroAcaoUseCase
    {
        private readonly IMediator mediator;

        public ObterQuestionarioRegistroAcaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<QuestaoDto>> Executar(long questionarioId, long? registroAcaoId)
        {
            return
                await mediator
                .Send(new ObterQuestionarioRegistroAcaoQuery(questionarioId, registroAcaoId));
        }
    }
}
