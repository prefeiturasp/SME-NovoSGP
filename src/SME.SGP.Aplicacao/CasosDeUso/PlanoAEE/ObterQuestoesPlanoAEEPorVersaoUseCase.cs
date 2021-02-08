using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesPlanoAEEPorVersaoUseCase : AbstractUseCase, IObterQuestoesPlanoAEEPorVersaoUseCase
    {
        public ObterQuestoesPlanoAEEPorVersaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<QuestaoDto>> Executar(long versaoPlanoId)
        {
            var respostasPlano = await mediator.Send(new ObterRespostasPlanoAEEPorVersaoQuery(versaoPlanoId));

            var questionarioId = await mediator.Send(new ObterQuestionarioPlanoAEEIdQuery());

            return await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(questionarioId, questaoId =>
               respostasPlano.Where(c => c.QuestaoId == questaoId)));
        }
    }
}
