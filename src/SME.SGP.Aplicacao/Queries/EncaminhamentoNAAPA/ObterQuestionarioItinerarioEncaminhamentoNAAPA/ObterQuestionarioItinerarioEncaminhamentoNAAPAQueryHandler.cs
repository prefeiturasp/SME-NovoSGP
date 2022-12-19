using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioItinerarioEncaminhamentoNAAPAQueryHandler : IRequestHandler<ObterQuestionarioItinerarioEncaminhamentoNAAPAQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamento;

        public ObterQuestionarioItinerarioEncaminhamentoNAAPAQueryHandler(IMediator mediator, IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamento, IRepositorioQuestionario repositorioQuestionario)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoEncaminhamento = repositorioQuestaoEncaminhamento ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamento));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestionarioItinerarioEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            var respostasEncaminhamento = request.EncaminhamentoSecaoId.HasValue ?
                                        await repositorioQuestaoEncaminhamento.ObterRespostasItinerarioEncaminhamento(request.EncaminhamentoSecaoId.Value) :
                                        Enumerable.Empty<RespostaQuestaoEncaminhamentoNAAPADto>();

            return await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.QuestionarioId, questaoId =>
                respostasEncaminhamento.Where(c => c.QuestaoId == questaoId)
                .Select(respostaEncaminhamento =>
                {
                    return new RespostaQuestaoDto()
                    {
                        Id = respostaEncaminhamento.Id,
                        OpcaoRespostaId = respostaEncaminhamento.RespostaId,
                        Texto = respostaEncaminhamento.Texto,
                        Arquivo = respostaEncaminhamento.Arquivo
                    };
                })));
        }
    }
}
