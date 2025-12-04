using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioItinerarioAtendimentoNAAPAQueryHandler : IRequestHandler<ObterQuestionarioItinerarioAtendimentoNAAPAQuery, AtendimentoNAAPASecaoItineranciaQuestoesDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamento;
        private readonly IRepositorioAtendimentoNAAPASecao repositorioEncaminhamentoNAAPASecao;

        public ObterQuestionarioItinerarioAtendimentoNAAPAQueryHandler(
                                                        IMediator mediator, 
                                                        IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamento,
                                                        IRepositorioAtendimentoNAAPASecao repositorioEncaminhamentoNAAPASecao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoEncaminhamento = repositorioQuestaoEncaminhamento ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamento));
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
        }

        public async Task<AtendimentoNAAPASecaoItineranciaQuestoesDto> Handle(ObterQuestionarioItinerarioAtendimentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            var encaminhamento = new AtendimentoNAAPASecaoItineranciaQuestoesDto();
            var respostasEncaminhamento = request.EncaminhamentoSecaoId.HasValue ?
                                        await repositorioQuestaoEncaminhamento.ObterRespostasItinerarioEncaminhamento(request.EncaminhamentoSecaoId.Value) :
                                        Enumerable.Empty<RespostaQuestaoAtendimentoNAAPADto>();

            if (respostasEncaminhamento.Any())
                encaminhamento.Auditoria = await repositorioEncaminhamentoNAAPASecao.ObterAuditoriaEncaminhamentoNaapaSecao(request.EncaminhamentoSecaoId.Value);
                
            encaminhamento.Questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.QuestionarioId, questaoId =>
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

            return encaminhamento;
        }
    }
}
