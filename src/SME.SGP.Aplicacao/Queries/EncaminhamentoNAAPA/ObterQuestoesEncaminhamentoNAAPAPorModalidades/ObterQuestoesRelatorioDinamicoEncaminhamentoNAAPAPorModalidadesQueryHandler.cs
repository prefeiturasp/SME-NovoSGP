using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Queries;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQueryHandler : IRequestHandler<ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;
        private const string SECAO_INFORMACOES_ESTUDANTE = "INFORMACOES_ESTUDANTE";

        public ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery request, CancellationToken cancellationToken)
        {
            var secoesQuestionario = await mediator.Send(new ObterSecoesEncaminhamentosSecaoNAAPAQuery(request.ModalidadeId, null));

            var secoesQuestionariosNAAPA = secoesQuestionario.Where(secao => secao.TipoQuestionario == TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA);

            if (!request.ModalidadeId.HasValue)
                secoesQuestionariosNAAPA = secoesQuestionariosNAAPA.Where(secao => secao.NomeComponente.Equals(SECAO_INFORMACOES_ESTUDANTE));

            var questoes = new List<QuestaoDto>();
            
            foreach (var secaoQuestionario in secoesQuestionariosNAAPA)
                questoes.AddRange(await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secaoQuestionario.QuestionarioId)));

            return questoes;
        }
    }
}
