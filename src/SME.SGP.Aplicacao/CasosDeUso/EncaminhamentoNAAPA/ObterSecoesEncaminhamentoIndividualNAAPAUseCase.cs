using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EncaminhamentoNAAPA
{
    public class ObterSecoesEncaminhamentoIndividualNAAPAUseCase : IObterSecoesAtendimentoIndividualNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterSecoesEncaminhamentoIndividualNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Executar(long? EncaminhamentoNAAPAId)
        {
            var secoesQuestionario = (await mediator.Send(new ObterSecaoEncaminhamentoIndividualQuery(EncaminhamentoNAAPAId))).ToList();

            foreach (var secao in secoesQuestionario)
            {
                var listaQuestoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secao.QuestionarioId));
                secao.Concluido = !listaQuestoes.Any(c => c.Obrigatorio);
            }

            return secoesQuestionario;
        }
    }
}

