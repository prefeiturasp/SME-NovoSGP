using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.MapeamentoEstudante;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioMapeamentoEstudanteUseCase : IObterQuestionarioMapeamentoEstudanteUseCase
    {
        private readonly IMediator mediator;

        public ObterQuestionarioMapeamentoEstudanteUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<QuestaoDto>> Executar(long questionarioId, long? mapeamentoEstudanteId)
        {
            return
                await mediator
                .Send(new ObterQuestionarioMapeamentoEstudanteQuery(questionarioId, mapeamentoEstudanteId));
        }
    }
}
