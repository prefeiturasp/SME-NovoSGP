﻿using Dapper;
using MediatR;
using Org.BouncyCastle.Ocsp;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
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
        private readonly IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE;

        public RegistrarEncaminhamentoAEEUseCase(IMediator mediator, IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoAEE = repositorioRespostaEncaminhamentoAEE ?? throw new System.ArgumentNullException(nameof(repositorioRespostaEncaminhamentoAEE));
        }

        public async Task<ResultadoEncaminhamentoAEEDto> Executar(EncaminhamentoAeeDto encaminhamentoAEEDto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEEDto.TurmaId));
            if (turma == null)
                throw new NegocioException("A turma informada não foi encontrada");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(encaminhamentoAEEDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno == null)
                throw new NegocioException("O aluno informado não foi encontrado");

            var alunoEncaminhamentoAEE = await mediator.Send(new ExisteEncaminhamentoAEEPorEstudanteQuery(encaminhamentoAEEDto.AlunoCodigo, turma.Ue.Id));
            if (alunoEncaminhamentoAEE && encaminhamentoAEEDto.Id == 0)
                throw new NegocioException("Estudante/Criança já possui encaminhamento AEE em aberto");

            if (!encaminhamentoAEEDto.Secoes.Any())
                throw new NegocioException("Nenhuma seção foi encontrada");

            if(encaminhamentoAEEDto.Situacao != SituacaoAEE.Encaminhado || encaminhamentoAEEDto.Secoes.Any(s => s.Concluido == false))
                await ValidarQuestoesObrigatoriasNaoPreechidas(encaminhamentoAEEDto);

            if (encaminhamentoAEEDto.Id.GetValueOrDefault() > 0)
            {
                var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(encaminhamentoAEEDto.Id.GetValueOrDefault()));
                if (encaminhamentoAEE != null)
                {
                    await AlterarEncaminhamento(encaminhamentoAEEDto, encaminhamentoAEE);
                    await RemoverArquivosNaoUtilizados(encaminhamentoAEEDto.Secoes);

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

        private async Task RemoverArquivosNaoUtilizados(List<EncaminhamentoAEESecaoDto> secoes)
        {
            var resposta = new List<EncaminhamentoAEESecaoQuestaoDto>();
            foreach (var s in secoes)
            {
                foreach (var q in s.Questoes)
                {
                    if (string.IsNullOrEmpty(q.Resposta) && q.TipoQuestao == TipoQuestao.Upload)
                    {
                        resposta.Add(q);
                    }
                }
            }

            if (resposta != null && resposta.Any())
            {
                foreach (var item in resposta)
                {
                    var entidadeResposta = repositorioRespostaEncaminhamentoAEE.ObterPorId(item.RespostaEncaminhamentoId);
                    if (entidadeResposta != null)
                    {
                        await mediator.Send(new ExcluirRespostaEncaminhamentoAEECommand(entidadeResposta));
                    }

                }
            }
        }
        public async Task AlterarEncaminhamento(EncaminhamentoAeeDto encaminhamentoAEEDto, EncaminhamentoAEE encaminhamentoAEE)
        {
            encaminhamentoAEE.Situacao = encaminhamentoAEEDto.Situacao;
            await mediator.Send(new SalvarEncaminhamentoAEECommand(encaminhamentoAEE));

            if (encaminhamentoAEEDto.Situacao != SituacaoAEE.Encaminhado &&
                encaminhamentoAEEDto.Situacao != SituacaoAEE.Analise &&
                encaminhamentoAEEDto.Situacao != SituacaoAEE.Rascunho)
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
        {
            var retorno = questaoExistente.Respostas.Where(s => !respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));
            return retorno;
        }

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


        private bool NaoNuloEContemRegistros(IEnumerable<dynamic> data)
        {
            return data != null && data.Any();
        }


        private bool EhQuestaoObrigatoriaNaoRespondida(QuestaoDto questao)
        {
            return questao.Obrigatorio &&
                    !NaoNuloEContemRegistros(questao.Resposta);
        }
        private void ValidaRecursivo(string secao, string questaoPaiOrdem, IEnumerable<QuestaoDto> questoes, List<dynamic> questoesObrigatoriasNaoRespondidas)
        {
            foreach (var questao in questoes)
            {
                var ordem = (questaoPaiOrdem != "" ? $"{questaoPaiOrdem}.{questao.Ordem.ToString()}" : questao.Ordem.ToString());

                if (EhQuestaoObrigatoriaNaoRespondida(questao)) {
                    questoesObrigatoriasNaoRespondidas.Add(new { Secao = secao, Ordem = ordem });
                }
                else
                if (NaoNuloEContemRegistros(questao.OpcaoResposta) 
                    && NaoNuloEContemRegistros(questao.Resposta))
                {
                    foreach (var resposta in questao.Resposta)
                    {
                        var opcao = questao.OpcaoResposta.Where(opcao => opcao.Id == Convert.ToInt64(resposta.Texto)).FirstOrDefault();
                        if (opcao != null && opcao.QuestoesComplementares.Any())
                        {
                            ValidaRecursivo(secao, ordem, opcao.QuestoesComplementares, questoesObrigatoriasNaoRespondidas);
                        }
                    }
                }
            }
        }

        private async Task ValidarQuestoesObrigatoriasNaoPreechidas(EncaminhamentoAeeDto encaminhamentoAEEDto)
        {
            var secoesEtapa = await this.mediator.Send(new ObterSecoesEncaminhamentoDtoPorEtapaQuery(1));
            var respostasEncaminhamento = encaminhamentoAEEDto.Secoes.Where(sessao => sessao.Questoes.Any())
                                                        .SelectMany(secao => secao.Questoes,
                                                                    (secao, questao) => new { questao.QuestaoId, questao.Resposta, questao.RespostaEncaminhamentoId })
                                                        .Where(questao => !string.IsNullOrEmpty(questao.Resposta));

            var questoesObrigatoriasNaorespondidas = new List<dynamic>();
            foreach (var secao in secoesEtapa)
            {
                var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secao.QuestionarioId, questaoId =>
                    respostasEncaminhamento.Where(c => c.QuestaoId == questaoId)
                    .Select(respostaEncaminhamento =>
                    {
                        return new RespostaQuestaoDto()
                        {
                            Id = GetHashCode(),
                            OpcaoRespostaId = 0,
                            Texto = respostaEncaminhamento.Resposta,
                            Arquivo = null
                        };
                    })));

                if (!questoes.Any(questao => questao.Obrigatorio)) { continue; }
                ValidaRecursivo(secao.Nome, "", questoes, questoesObrigatoriasNaorespondidas);
            }

            if (questoesObrigatoriasNaorespondidas.Any() && encaminhamentoAEEDto.Situacao != SituacaoAEE.Rascunho)
            {
                var mensagem = new List<string>();
                foreach (var secao in questoesObrigatoriasNaorespondidas.GroupBy(questao => questao.Secao))
                {
                    mensagem.Add($"Seção: {secao.Key} Questões: [{string.Join(", ", secao.Select(questao => questao.Ordem).Distinct().ToArray())}]");
                }
                throw new NegocioException(String.Format(MensagemNegocioEncaminhamentoAee.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                                                String.Join(", ", mensagem)));
            }
        }
    }
}


