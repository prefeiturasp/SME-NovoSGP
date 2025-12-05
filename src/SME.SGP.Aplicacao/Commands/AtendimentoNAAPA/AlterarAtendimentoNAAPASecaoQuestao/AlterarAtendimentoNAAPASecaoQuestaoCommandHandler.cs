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
    public class AlterarAtendimentoNAAPASecaoQuestaoCommandHandler : IRequestHandler<AlterarAtendimentoNAAPASecaoQuestaoCommand, bool>
    {
        private readonly IMediator mediator;

        public AlterarAtendimentoNAAPASecaoQuestaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(AlterarAtendimentoNAAPASecaoQuestaoCommand request, CancellationToken cancellationToken)
        {
            var questoesExistentes = request.EncaminhamentoNAAPASecaoObj.Questoes;
            
            var questoesRespondidas = request.EncaminhamentoNAAPASecaoDto.Questoes; 
            var questoesRespondidasAgrupadas = questoesRespondidas.GroupBy(q => q.QuestaoId);
            
            foreach (var questoes in questoesRespondidasAgrupadas)
            {
                var questaoExistente = questoesExistentes.FirstOrDefault(q => q.QuestaoId == questoes.FirstOrDefault().QuestaoId);
                if (questaoExistente.EhNulo())
                {
                    var resultadoEncaminhamentoQuestao = await mediator.Send(
                        new RegistrarAtendimentoNAAPASecaoQuestaoCommand(request.EncaminhamentoNAAPASecaoObj.Id,
                            questoes.Key), cancellationToken);
                    
                    await RegistrarRespostaEncaminhamento(questoes, resultadoEncaminhamentoQuestao);
                }
                else
                    await AlterarQuestoesExistentes(questaoExistente, questoes);
            }
            await ExcluirQuestoesExistentes(questoesExistentes.Where(x => questoesRespondidas.All(s => s.QuestaoId != x.QuestaoId)));
            
            return true;
        }

        private async Task ExcluirQuestoesExistentes(IEnumerable<QuestaoEncaminhamentoNAAPA> questoesRemovidas)
        {
            foreach (var questao in questoesRemovidas)
                await mediator.Send(new ExcluirQuestaoAtendimentoNAAPAPorIdCommand(questao.Id));
        }

        private async Task AlterarQuestoesExistentes(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, AtendimentoNAAPASecaoQuestaoDto> questoesRespostas)
        {
            if (questaoExistente.Excluido)
                await AlterarQuestaoExcluida(questaoExistente);
            await ExcluirRespostasEncaminhamento(questaoExistente, questoesRespostas);
            await AlterarRespostasEncaminhamento(questaoExistente, questoesRespostas);
            await IncluirRespostasEncaminhamento(questaoExistente, questoesRespostas);
        }

        private async Task RegistrarRespostaEncaminhamento(IEnumerable<AtendimentoNAAPASecaoQuestaoDto> questoes, long questaoEncaminhamentoId)
        {
            foreach (var questao in questoes)
                await mediator.Send(new RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommand(questao.Resposta, questaoEncaminhamentoId, questao.TipoQuestao));
        }

        private async Task AlterarQuestaoExcluida(QuestaoEncaminhamentoNAAPA questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarQuestaoAtendimentoNAAPACommand(questao));
        }

        private async Task ExcluirRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, AtendimentoNAAPASecaoQuestaoDto> respostas)
        {
            foreach (var respostasExcluir in ObterRespostasAExcluir(questaoExistente, respostas))
                await mediator.Send(new ExcluirRespostaAtendimentoNAAPACommand(respostasExcluir));
        }

        private async Task AlterarRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, AtendimentoNAAPASecaoQuestaoDto> respostas)
        {
            foreach (var respostaAlterar in ObterRespostasAAlterar(questaoExistente, respostas))
                await mediator.Send(new AlterarAtendimentoNAAPASecaoQuestaoRespostaCommand(respostaAlterar, respostas.FirstOrDefault(c => c.RespostaEncaminhamentoId == respostaAlterar.Id)));
        }

        private IEnumerable<RespostaEncaminhamentoNAAPA> ObterRespostasAAlterar(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, AtendimentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
            => questaoExistente.Respostas.Where(s => respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));

        private IEnumerable<RespostaEncaminhamentoNAAPA> ObterRespostasAExcluir(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, AtendimentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
            => questaoExistente.Respostas.Where(s => respostasEncaminhamento.All(c => c.RespostaEncaminhamentoId != s.Id));

        private async Task IncluirRespostasEncaminhamento(EntidadeBase questaoExistente, IEnumerable<AtendimentoNAAPASecaoQuestaoDto> respostas)
            => await RegistrarRespostaEncaminhamento(ObterRespostasAIncluir(respostas), questaoExistente.Id);

        private IEnumerable<AtendimentoNAAPASecaoQuestaoDto> ObterRespostasAIncluir(IEnumerable<AtendimentoNAAPASecaoQuestaoDto> respostas)
            => respostas.Where(c => c.RespostaEncaminhamentoId == 0);
    }
}
