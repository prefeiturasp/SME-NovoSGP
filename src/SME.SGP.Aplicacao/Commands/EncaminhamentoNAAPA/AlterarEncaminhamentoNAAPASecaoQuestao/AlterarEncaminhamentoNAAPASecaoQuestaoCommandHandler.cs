using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarEncaminhamentoNAAPASecaoQuestaoCommandHandler : IRequestHandler<AlterarEncaminhamentoNAAPASecaoQuestaoCommand, bool>
    {
        private readonly IMediator mediator;

        public AlterarEncaminhamentoNAAPASecaoQuestaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(AlterarEncaminhamentoNAAPASecaoQuestaoCommand request, CancellationToken cancellationToken)
        {
            foreach (var questoes in request.EncaminhamentoNAAPASecaoDto.Questoes.GroupBy(q => q.QuestaoId))
            {
                var questaoExistente = request.EncaminhamentoNAAPASecaoObj.Questoes.FirstOrDefault(q => q.QuestaoId == questoes.FirstOrDefault().QuestaoId);

                if (questaoExistente == null)
                {
                    var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoQuestaoCommand(request.EncaminhamentoNAAPASecaoObj.Id, questoes.FirstOrDefault().QuestaoId));
                    await RegistrarRespostaEncaminhamento(questoes, resultadoEncaminhamentoQuestao);
                }
                else
                {
                    if (questaoExistente.Excluido)
                        await AlterarQuestaoExcluida(questaoExistente);

                    await ExcluirRespostasEncaminhamento(questaoExistente, questoes);

                    await AlterarRespostasEncaminhamento(questaoExistente, questoes);

                    await IncluirRespostasEncaminhamento(questaoExistente, questoes);
                }
            }

            return true;
        }

        private async Task RegistrarRespostaEncaminhamento(IEnumerable<EncaminhamentoNAAPASecaoQuestaoDto> questoes, long questaoEncaminhamentoId)
        {
            foreach (var questao in questoes)
            {
                await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(questao.Resposta, questaoEncaminhamentoId, questao.TipoQuestao));
            }
        }

        private async Task AlterarQuestaoExcluida(QuestaoEncaminhamentoNAAPA questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarQuestaoEncaminhamentoNAAPACommand(questao));
        }

        private async Task ExcluirRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questoesExistentes, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas)
        {
            foreach (var respostasExcluir in ObterRespostasAExcluir(questoesExistentes, respostas))
                await mediator.Send(new ExcluirRespostaEncaminhamentoNAAPACommand(respostasExcluir));
        }

        private async Task AlterarRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas)
        {
            foreach (var respostaAlterar in ObterRespostasAAlterar(questaoExistente, respostas))
                await mediator.Send(new AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(respostaAlterar, respostas.FirstOrDefault(c => c.RespostaEncaminhamentoId == respostaAlterar.Id)));
        }

        private IEnumerable<RespostaEncaminhamentoNAAPA> ObterRespostasAAlterar(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
                => questaoExistente.Respostas.Where(s => respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));


        private IEnumerable<RespostaEncaminhamentoNAAPA> ObterRespostasAExcluir(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
                => questaoExistente.Respostas.Where(s => !respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));

        private async Task IncluirRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas)
                => await RegistrarRespostaEncaminhamento(ObterRespostasAIncluir(questaoExistente, respostas), questaoExistente.Id);

        private IEnumerable<EncaminhamentoNAAPASecaoQuestaoDto> ObterRespostasAIncluir(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas)
                => respostas.Where(c => c.RespostaEncaminhamentoId == 0);
    }
}
