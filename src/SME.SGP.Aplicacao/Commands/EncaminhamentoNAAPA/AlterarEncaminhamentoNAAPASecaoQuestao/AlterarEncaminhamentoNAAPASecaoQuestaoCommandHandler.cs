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
            var questoesExistentes = request.EncaminhamentoNAAPASecaoObj.Questoes;
            var questoesExistentesAgrupadas = questoesExistentes.GroupBy(q => q.QuestaoId).ToList();
            
            var questoesRespondidas = request.EncaminhamentoNAAPASecaoDto.Questoes; 
            var questoesRespondidasAgrupadas = questoesRespondidas.GroupBy(q => q.QuestaoId);
            
            foreach (var questoes in questoesRespondidasAgrupadas)
            {
                var questaoExistenteAgrupada = questoesExistentesAgrupadas.FirstOrDefault(c => c.Key == questoes.Key);

                if (questaoExistenteAgrupada.EhNulo())
                {
                    var resultadoEncaminhamentoQuestao = await mediator.Send(
                        new RegistrarEncaminhamentoNAAPASecaoQuestaoCommand(request.EncaminhamentoNAAPASecaoObj.Id,
                            questoes.Key), cancellationToken);
                    
                    await RegistrarRespostaEncaminhamento(questoes, resultadoEncaminhamentoQuestao);
                }
                else
                {
                    foreach (var questaoExistente in questaoExistenteAgrupada)
                    {
                        if (questaoExistente.Excluido)
                            await AlterarQuestaoExcluida(questaoExistente);

                        await ExcluirRespostasEncaminhamento(questaoExistente, questoes);

                        await AlterarRespostasEncaminhamento(questaoExistente, questoes);

                        await IncluirRespostasEncaminhamento(questaoExistente, questoes);                        
                    }
                }
            }
            
            foreach (var questao in questoesExistentes.Where(x => questoesRespondidas.All(s => s.QuestaoId != x.QuestaoId)))
                await mediator.Send(new ExcluirQuestaoEncaminhamentoNAAPAPorIdCommand(questao.Id), cancellationToken);            
            
            return true;
        }

        private async Task RegistrarRespostaEncaminhamento(IEnumerable<EncaminhamentoNAAPASecaoQuestaoDto> questoes, long questaoEncaminhamentoId)
        {
            foreach (var questao in questoes)
                await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(questao.Resposta, questaoEncaminhamentoId, questao.TipoQuestao));
        }

        private async Task AlterarQuestaoExcluida(QuestaoEncaminhamentoNAAPA questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarQuestaoEncaminhamentoNAAPACommand(questao));
        }

        private async Task ExcluirRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas)
        {
            foreach (var respostasExcluir in ObterRespostasAExcluir(questaoExistente, respostas))
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
            => questaoExistente.Respostas.Where(s => respostasEncaminhamento.All(c => c.RespostaEncaminhamentoId != s.Id));

        private async Task IncluirRespostasEncaminhamento(EntidadeBase questaoExistente, IEnumerable<EncaminhamentoNAAPASecaoQuestaoDto> respostas)
            => await RegistrarRespostaEncaminhamento(ObterRespostasAIncluir(respostas), questaoExistente.Id);

        private IEnumerable<EncaminhamentoNAAPASecaoQuestaoDto> ObterRespostasAIncluir(IEnumerable<EncaminhamentoNAAPASecaoQuestaoDto> respostas)
            => respostas.Where(c => c.RespostaEncaminhamentoId == 0);
    }
}
