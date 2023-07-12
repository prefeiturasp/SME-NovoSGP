using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioPAPQueryHandler : IRequestHandler<ObterQuestionarioPAPQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRelatorioPeriodicoPAPResposta repositorio;

        public ObterQuestionarioPAPQueryHandler(IMediator mediator, IRepositorioRelatorioPeriodicoPAPResposta repositorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestionarioPAPQuery request, CancellationToken cancellationToken)
        {
            var respostasEncaminhamento = request.PAPSecaoId.HasValue ?
                    await repositorio.ObterRespostas(request.PAPSecaoId.Value) :
                    Enumerable.Empty<RelatorioPeriodicoPAPResposta>();

            return await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.QuestionarioId, questaoId =>
                respostasEncaminhamento.Where(c => c.RelatorioPeriodicoQuestao.QuestaoId == questaoId)
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
