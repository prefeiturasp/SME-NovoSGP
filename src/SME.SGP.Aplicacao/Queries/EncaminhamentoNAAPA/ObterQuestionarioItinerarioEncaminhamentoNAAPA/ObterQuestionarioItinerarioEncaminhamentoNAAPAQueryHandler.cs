using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioItinerarioEncaminhamentoNAAPAQueryHandler : IRequestHandler<ObterQuestionarioItinerarioEncaminhamentoNAAPAQuery, AtendimentoNAAPASecaoItineranciaQuestoesDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamento;
        private readonly IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao;

        public ObterQuestionarioItinerarioEncaminhamentoNAAPAQueryHandler(
                                                        IMediator mediator, 
                                                        IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamento,
                                                        IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoEncaminhamento = repositorioQuestaoEncaminhamento ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamento));
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
        }

        public async Task<AtendimentoNAAPASecaoItineranciaQuestoesDto> Handle(ObterQuestionarioItinerarioEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
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
