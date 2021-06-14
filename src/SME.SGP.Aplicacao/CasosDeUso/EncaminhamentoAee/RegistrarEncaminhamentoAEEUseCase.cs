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

            var alunoEncaminhamentoAEE = await mediator.Send(new ExisteEncaminhamentoAEEPorEstudanteQuery(encaminhamentoAEEDto.AlunoCodigo));
            if (alunoEncaminhamentoAEE && encaminhamentoAEEDto.Id == 0)
                throw new NegocioException("Estudante/Criança já possui encaminhamento AEE em aberto");

            if (!encaminhamentoAEEDto.Secoes.Any())
                throw new NegocioException("Nenhuma seção foi encontrada");

            if (encaminhamentoAEEDto.Id.GetValueOrDefault() > 0)
            {
                var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(encaminhamentoAEEDto.Id.GetValueOrDefault()));
                if (encaminhamentoAEE != null)
                {
                    await AlterarEncaminhamento(encaminhamentoAEEDto, encaminhamentoAEE);

                    if (await ParametroGeracaoPendenciaAtivo())
                        await mediator.Send(new GerarPendenciaCPEncaminhamentoAEECommand(encaminhamentoAEE.Id, encaminhamentoAEEDto.Situacao));

                    return new ResultadoEncaminhamentoAEEDto() { Id = encaminhamentoAEE.Id };
                }
            }

            var resultadoEncaminhamento = await mediator.Send(new RegistrarEncaminhamentoAeeCommand(
            encaminhamentoAEEDto.TurmaId, aluno.NomeAluno, aluno.CodigoAluno,
            encaminhamentoAEEDto.Situacao));

            await SalvarEncaminhamento(encaminhamentoAEEDto, resultadoEncaminhamento);

            if (await ParametroGeracaoPendenciaAtivo())
                await mediator.Send(new GerarPendenciaCPEncaminhamentoAEECommand(resultadoEncaminhamento.Id, encaminhamentoAEEDto.Situacao));

            return resultadoEncaminhamento;
        }

        private Task<bool> EhUsuarioResponsavelPeloEncaminhamento(Usuario usuarioLogado, long? responsavelId)
            => Task.FromResult(responsavelId.HasValue && usuarioLogado.Id == responsavelId.Value);

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasEncaminhamentoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }

        public async Task AlterarEncaminhamento(EncaminhamentoAeeDto encaminhamentoAEEDto, EncaminhamentoAEE encaminhamentoAEE)
        {
            encaminhamentoAEE.Situacao = encaminhamentoAEEDto.Situacao;
            await mediator.Send(new SalvarEncaminhamentoAEECommand(encaminhamentoAEE));

            if (encaminhamentoAEEDto.Situacao != SituacaoAEE.Encaminhado)
            {
                await mediator.Send(new ExcluirPendenciasEncaminhamentoAEECPCommand(encaminhamentoAEE.TurmaId, encaminhamentoAEE.Id));
            }

            foreach (var secao in encaminhamentoAEEDto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException($"Nenhuma questão foi encontrada na Seção {secao.SecaoId}");

                var secaoExistente = encaminhamentoAEE.Secoes.FirstOrDefault(s => s.SecaoEncaminhamentoAEEId == secao.SecaoId);

                long resultadoEncaminhamentoSecao = 0;
                if (secaoExistente == null)
                    secaoExistente = await mediator.Send(new RegistrarEncaminhamentoAEESecaoCommand(encaminhamentoAEE.Id, secao.SecaoId, secao.Concluido));
                else
                {
                    secaoExistente.Concluido = secao.Concluido;
                    await mediator.Send(new AlterarEncaminhamentoAEESecaoCommand(secaoExistente));
                }

                resultadoEncaminhamentoSecao = secaoExistente.Id;

                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var questaoExistente = secaoExistente.Questoes.FirstOrDefault(q => q.QuestaoId == questoes.FirstOrDefault().QuestaoId);

                    if (questaoExistente == null)
                    {
                        var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarEncaminhamentoAEESecaoQuestaoCommand(resultadoEncaminhamentoSecao, questoes.FirstOrDefault().QuestaoId));
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

                foreach (var questao in secaoExistente.Questoes.Where(x => !secao.Questoes.Any(s => s.QuestaoId == x.QuestaoId)))
                    await mediator.Send(new ExcluirQuestaoEncaminhamentoAEEPorIdCommand(questao.Id));
            }
        }

        private async Task AlterarQuestaoExcluida(QuestaoEncaminhamentoAEE questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarQuestaoEncaminhamentoAEECommand(questao));
        }

        private async Task IncluirRespostasEncaminhamento(QuestaoEncaminhamentoAEE questaoExistente, IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> respostas)
            => await RegistrarRespostaEncaminhamento(ObterRespostasAIncluir(questaoExistente, respostas), questaoExistente.Id);

        private async Task RegistrarRespostaEncaminhamento(IEnumerable<EncaminhamentoAEESecaoQuestaoDto> questoes, long questaoEncaminhamentoId)
        {
            foreach (var questao in questoes)
            {
                await mediator.Send(new RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand(questao.Resposta, questaoEncaminhamentoId, questao.TipoQuestao));
            }
        }

        private async Task AlterarRespostasEncaminhamento(QuestaoEncaminhamentoAEE questaoExistente, IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> respostas)
        {
            foreach (var respostaAlterar in ObterRespostasAAlterar(questaoExistente, respostas))
                await mediator.Send(new AlterarEncaminhamentoAEESecaoQuestaoRespostaCommand(respostaAlterar, respostas.FirstOrDefault(c => c.RespostaEncaminhamentoId == respostaAlterar.Id)));
        }

        private async Task ExcluirRespostasEncaminhamento(QuestaoEncaminhamentoAEE questoesExistentes, IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> respostas)
        {
            foreach (var respostasExcluir in ObterRespostasAExcluir(questoesExistentes, respostas))
                await mediator.Send(new ExcluirRespostaEncaminhamentoAEECommand(respostasExcluir));
        }

        private IEnumerable<EncaminhamentoAEESecaoQuestaoDto> ObterRespostasAIncluir(QuestaoEncaminhamentoAEE questaoExistente, IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> respostas)
            => respostas.Where(c => c.RespostaEncaminhamentoId == 0);

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

                var secaoEncaminhamento = await mediator.Send(new RegistrarEncaminhamentoAEESecaoCommand(resultadoEncaminhamento.Id, secao.SecaoId, secao.Concluido));

                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarEncaminhamentoAEESecaoQuestaoCommand(secaoEncaminhamento.Id, questoes.FirstOrDefault().QuestaoId));
                    await RegistrarRespostaEncaminhamento(questoes, resultadoEncaminhamentoQuestao);
                }
            }
        }
    }
}


