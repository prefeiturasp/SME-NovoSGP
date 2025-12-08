using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterQuestionarioItinerarioAtendimentoNAAPAUseCase : IObterQuestionarioItinerarioAtendimentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterQuestionarioItinerarioAtendimentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AtendimentoNAAPASecaoItineranciaQuestoesDto> Executar(long questionarioId, long? encaminhamentoSecaoId)
        {
            return await mediator
                        .Send(new ObterQuestionarioItinerarioAtendimentoNAAPAQuery(questionarioId, encaminhamentoSecaoId));
        }
    }
}
