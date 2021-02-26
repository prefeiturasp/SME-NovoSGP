using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesPlanoAEEPorVersaoUseCase : AbstractUseCase, IObterQuestoesPlanoAEEPorVersaoUseCase
    {
        public ObterQuestoesPlanoAEEPorVersaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<QuestaoDto>> Executar(FiltroPesquisaQuestoesPlanoAEEDto filtro)
        {
            var respostasPlano = await mediator.Send(new ObterRespostasPlanoAEEPorVersaoQuery(filtro.VersaoPlanoId));

            var questionarioId = await mediator.Send(new ObterQuestionarioPlanoAEEIdQuery());

            var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(questionarioId, questaoId =>
               respostasPlano.Where(c => c.QuestaoId == questaoId)));

            questoes = await AplicarRegrasPlano(filtro.TurmaCodigo, questoes);

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
