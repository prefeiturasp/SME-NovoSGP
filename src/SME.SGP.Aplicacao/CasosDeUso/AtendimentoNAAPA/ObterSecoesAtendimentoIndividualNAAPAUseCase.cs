using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EncaminhamentoNAAPA
{
    public class ObterSecoesAtendimentoIndividualNAAPAUseCase : IObterSecoesAtendimentoIndividualNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterSecoesAtendimentoIndividualNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Executar(long? EncaminhamentoNAAPAId, long? tipoQuestionario = null)
        {
            var secoesQuestionario = (await mediator.Send(new ObterSecaoAtendimentoIndividualQuery(EncaminhamentoNAAPAId, tipoQuestionario))).ToList();

            foreach (var secao in secoesQuestionario)
            {
                var listaQuestoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secao.QuestionarioId));
                secao.Concluido = !listaQuestoes.Any(c => c.Obrigatorio);
            }

            return secoesQuestionario;
        }
    }
}

