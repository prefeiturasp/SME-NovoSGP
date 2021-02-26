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
            var respostasPlano = await mediator.Send(new ObterRespostasPlanoAEEPorVersaoQuery(request.VersaoPlanoId));

            var questionarioId = await mediator.Send(new ObterQuestionarioPlanoAEEIdQuery());

            var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(questionarioId, questaoId =>
               respostasPlano.Where(c => c.QuestaoId == questaoId)));

            questoes = await AplicarRegrasPlano(request.TurmaCodigo, questoes);

            return questoes;
        }

        private async Task<IEnumerable<QuestaoDto>> AplicarRegrasPlano(string turmaCodigo, IEnumerable<QuestaoDto> questoes)
        {
            var codigos = await mediator.Send(new ObteCodigosDreUePorTurmaQuery(turmaCodigo));
            var funciorarioPAEE = await mediator.Send(new ObterPAEETurmaQuery(codigos.DreCodigo, codigos.UeCodigo));

            if (funciorarioPAEE != null && funciorarioPAEE.Any())
            {
                var questao = ObterQuestaoOrganizacao(questoes);
                questao.OpcaoResposta = RemoverOpcaoItinerante(questao.OpcaoResposta);
            }

            return questoes;
        }

        private OpcaoRespostaDto[] RemoverOpcaoItinerante(OpcaoRespostaDto[] opcaoResposta)
            => opcaoResposta.Where(c => c.Ordem != 3).ToArray();

        private QuestaoDto ObterQuestaoOrganizacao(IEnumerable<QuestaoDto> questoes)
            => questoes.FirstOrDefault(c => c.Ordem == 2);
    }
}
