using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RegistrarEncaminhamentoAEEUseCase : IRegistrarEncaminhamentoAEEUseCase
    {
        private readonly IMediator mediator;

        public RegistrarEncaminhamentoAEEUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoEncaminhamentoAEEDto> Executar(EncaminhamentoAeeDto encaminhamentoAEEDto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEEDto.TurmaId));
            if (turma == null)
                throw new NegocioException("A turma informada não foi encontrada");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(encaminhamentoAEEDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno == null)
                throw new NegocioException("O aluno informado não foi encontrado");

            if (!encaminhamentoAEEDto.Secoes.Any())
                throw new NegocioException("Nenhuma seção foi encontrada");

            var encaminhamentoConcluido = encaminhamentoAEEDto.Secoes.Where(c => !c.Concluido).Any();

            if (encaminhamentoAEEDto.Id.GetValueOrDefault() > 0)
            {
                var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(encaminhamentoAEEDto.Id.GetValueOrDefault()));
                if (encaminhamentoAEE != null)
                {
                    await AlterarEncaminhamento(encaminhamentoAEEDto, encaminhamentoAEE, encaminhamentoConcluido);
                    return new ResultadoEncaminhamentoAEEDto() { Id = encaminhamentoAEE.Id };
                }
            }

            var resultadoEncaminhamento = await mediator.Send(new RegistrarEncaminhamentoAeeCommand(
            encaminhamentoAEEDto.TurmaId, aluno.NomeAluno, aluno.CodigoAluno,
            encaminhamentoConcluido ? SituacaoAEE.Rascunho : SituacaoAEE.Encaminhado));


            await SalvarEncaminhamento(encaminhamentoAEEDto, resultadoEncaminhamento);
            return resultadoEncaminhamento;
        }

        public async Task AlterarEncaminhamento(EncaminhamentoAeeDto encaminhamentoAEEDto, EncaminhamentoAEE encaminhamentoAEE, bool encaminhamentoConcluido)
        {
            foreach (var secao in encaminhamentoAEEDto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException($"Nenhuma questão foi encontrada na Seção {secao.SecaoId}");

                var secaoExistente = encaminhamentoAEE.Secoes.FirstOrDefault(s => s.SecaoEncaminhamentoAEEId == secao.SecaoId);

                long resultadoEncaminhamentoSecao = 0;
                if (secaoExistente == null)
                    resultadoEncaminhamentoSecao = await mediator.Send(new RegistrarEncaminhamentoAEESecaoCommand(encaminhamentoAEE.Id, secao.SecaoId, secao.Concluido));
                else
                {
                    resultadoEncaminhamentoSecao = secaoExistente.Id;
                    if (secaoExistente.Concluido != secao.Concluido)
                    {
                        secaoExistente.Concluido = secao.Concluido;
                        await mediator.Send(new AlterarEncaminhamentoAEESecaoCommand(secaoExistente));
                    }
                }

                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var questoesExistentes = secaoExistente.Questoes.FirstOrDefault(q => q.QuestaoId == questoes.FirstOrDefault().QuestaoId);

                    if (questoesExistentes == null)
                    {
                        var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarEncaminhamentoAEESecaoQuestaoCommand(resultadoEncaminhamentoSecao, questoes.FirstOrDefault().QuestaoId));
                        await RegistrarRespostaEncaminhamento(questoes, resultadoEncaminhamentoQuestao);
                    }
                    else
                    {
                        await ExcluirRespostasEncaminhamento(questoesExistentes, questoes);

                        await AlterarRespostasEncaminhamento(questoesExistentes, questoes);
                    }
                }

                foreach (var questao in secaoExistente.Questoes.Where(x => !secao.Questoes.Any(s => s.QuestaoId == x.QuestaoId)))
                {
                    await mediator.Send(new ExcluirQuestaoEncaminhamentoAEEPorIdCommand(questao.Id));
                }
            }
        }

        private async Task RegistrarRespostaEncaminhamento(IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> questoes, long resultadoEncaminhamentoQuestao)
        {
            foreach (var q in questoes)
            {
                await mediator.Send(new RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand(q.Resposta, resultadoEncaminhamentoQuestao, q.TipoQuestao));
            }
        }

        private async Task AlterarRespostasEncaminhamento(QuestaoEncaminhamentoAEE questaoExistente, IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> respostas)
        {
            foreach (var respostaAlterar in ObterRespostasAAlterar(questaoExistente, respostas))
                await mediator.Send(new AlterarEncaminhamentoAEESecaoQuestaoRespostaCommand(respostaAlterar, respostas.FirstOrDefault(c => c.RespostaEncaminhamentoId == respostaAlterar.Id)));
        }

        private async Task ExcluirRespostasEncaminhamento(QuestaoEncaminhamentoAEE questoesExistentes, IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> questoes)
        {
            foreach (var respostasExcluir in ObterRespostasAExcluir(questoesExistentes, questoes))
                await mediator.Send(new ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommand(questoesExistentes.Id));
        }

        private IEnumerable<RespostaEncaminhamentoAEE> ObterRespostasAExcluir(QuestaoEncaminhamentoAEE questaoExistente, IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> respostasEncaminhamento)
            => questaoExistente.Respostas.Where(s => !respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));

        private IEnumerable<RespostaEncaminhamentoAEE> ObterRespostasAAlterar(QuestaoEncaminhamentoAEE questaoExistente, IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> respostasEncaminhamento)
            => questaoExistente.Respostas.Where(s => respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));

        public async Task SalvarEncaminhamento(EncaminhamentoAeeDto encaminhamentoAEEDto, ResultadoEncaminhamentoAEEDto resultadoEncaminhamento)
        {
            foreach (var secao in encaminhamentoAEEDto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException($"Nenhuma questão foi encontrada na Seção {secao.SecaoId}");

                var resultadoEncaminhamentoSecao = await mediator.Send(new RegistrarEncaminhamentoAEESecaoCommand(resultadoEncaminhamento.Id, secao.SecaoId, secao.Concluido));

                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarEncaminhamentoAEESecaoQuestaoCommand(resultadoEncaminhamentoSecao, questoes.FirstOrDefault().QuestaoId));
                    await RegistrarRespostaEncaminhamento(questoes, resultadoEncaminhamentoQuestao);
                }
            }
        }
    }
}


