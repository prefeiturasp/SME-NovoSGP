using MediatR;
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
        private readonly IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamento;

        public RegistrarEncaminhamentoAEEUseCase(IMediator mediator, IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamento, IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoAEE = repositorioRespostaEncaminhamentoAEE ?? throw new System.ArgumentNullException(nameof(repositorioRespostaEncaminhamentoAEE));
            this.repositorioQuestaoEncaminhamento = repositorioQuestaoEncaminhamento ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamento));
        }

        public async Task<ResultadoEncaminhamentoAEEDto> Executar(EncaminhamentoAeeDto encaminhamentoAEEDto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEEDto.TurmaId));
            if (turma.EhNulo())
                throw new NegocioException("A turma informada não foi encontrada");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(encaminhamentoAEEDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno.EhNulo())
                throw new NegocioException("O aluno informado não foi encontrado");

            var alunoEncaminhamentoAEE = await mediator.Send(new ExisteEncaminhamentoAEEPorEstudanteQuery(encaminhamentoAEEDto.AlunoCodigo, turma.Ue.Id));
            if (alunoEncaminhamentoAEE && encaminhamentoAEEDto.Id == 0)
                throw new NegocioException("Estudante/Criança já possui encaminhamento AEE em aberto");

            if (!encaminhamentoAEEDto.Secoes.Any())
                throw new NegocioException("Nenhuma seção foi encontrada");

            if (encaminhamentoAEEDto.Situacao != SituacaoAEE.Rascunho && encaminhamentoAEEDto.Secoes.Any(s => s.Concluido == false))
                await ValidarQuestoesObrigatoriasNaoPreechidas(encaminhamentoAEEDto);

            if (encaminhamentoAEEDto.Id.GetValueOrDefault() > 0)
            {
                var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(encaminhamentoAEEDto.Id.GetValueOrDefault()));
                if (encaminhamentoAEE.NaoEhNulo())
                {
                    await AlterarEncaminhamento(encaminhamentoAEEDto, encaminhamentoAEE);
                    await RemoverArquivosNaoUtilizados(encaminhamentoAEEDto.Secoes);

                    if (await ParametroGeracaoPendenciaAtivo())
                        await mediator.Send(new GerarPendenciaCPEncaminhamentoAEECommand(encaminhamentoAEE.Id, encaminhamentoAEEDto.Situacao));

                    await mediator.Send(new SalvarEncaminhamentoAEETurmaAlunoCommand(encaminhamentoAEE.Id, encaminhamentoAEE.AlunoCodigo));

                    return new ResultadoEncaminhamentoAEEDto() { Id = encaminhamentoAEE.Id };
                }
            }

            var resultadoEncaminhamento = await mediator.Send(new RegistrarEncaminhamentoAeeCommand(
            encaminhamentoAEEDto.TurmaId, aluno.NomeAluno, aluno.CodigoAluno,
            encaminhamentoAEEDto.Situacao));

            await SalvarEncaminhamento(encaminhamentoAEEDto, resultadoEncaminhamento);

            if (await ParametroGeracaoPendenciaAtivo())
                await mediator.Send(new GerarPendenciaCPEncaminhamentoAEECommand(resultadoEncaminhamento.Id, encaminhamentoAEEDto.Situacao));

            await mediator.Send(new SalvarEncaminhamentoAEETurmaAlunoCommand(resultadoEncaminhamento.Id, encaminhamentoAEEDto.AlunoCodigo));

            return resultadoEncaminhamento;
        }

        private Task<bool> EhUsuarioResponsavelPeloEncaminhamento(Usuario usuarioLogado, long? responsavelId)
            => Task.FromResult(responsavelId.HasValue && usuarioLogado.Id == responsavelId.Value);

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasEncaminhamentoAEE, DateTime.Today.Year));

            return parametro.NaoEhNulo() && parametro.Ativo;
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

            if (resposta.NaoEhNulo() && resposta.Any())
            {
                foreach (var item in resposta)
                {
                    var entidadeResposta = repositorioRespostaEncaminhamentoAEE.ObterPorId(item.RespostaEncaminhamentoId);
                    if (entidadeResposta.NaoEhNulo())
                    {
                        await mediator.Send(new ExcluirRespostaEncaminhamentoAEECommand(entidadeResposta));
                    }

                }
            }
        }

        private async Task ExcluirPendenciasEncaminhamentoAEE(SituacaoAEE situacaoEncaminhamentoAEE, EncaminhamentoAEE encaminhamentoAEE)
        {
            if (situacaoEncaminhamentoAEE.PermiteExclusaoPendenciasEncaminhamentoAEE())
                await mediator.Send(new ExcluirPendenciasEncaminhamentoAEECPCommand(encaminhamentoAEE.TurmaId, encaminhamentoAEE.Id));
        }

        private async Task AlterarQuestoesExistentes(QuestaoEncaminhamentoAEE questaoExistente, IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> questoesRespostas)
        {
            if (questaoExistente.Excluido)
                await AlterarQuestaoExcluida(questaoExistente);
            await ExcluirRespostasEncaminhamento(questaoExistente, questoesRespostas);
            await AlterarRespostasEncaminhamento(questaoExistente, questoesRespostas);
            await IncluirRespostasEncaminhamento(questaoExistente, questoesRespostas);
        }

        private async Task ExcluirQuestoesExistentes(IEnumerable<QuestaoEncaminhamentoAEE> questoesRemovidas)
        {
            foreach (var questao in questoesRemovidas)
                await mediator.Send(new ExcluirQuestaoEncaminhamentoAEEPorIdCommand(questao.Id));
        }

        public async Task AlterarEncaminhamento(EncaminhamentoAeeDto encaminhamentoAEEDto, EncaminhamentoAEE encaminhamentoAEE)
        {
            encaminhamentoAEE.Situacao = encaminhamentoAEEDto.Situacao;
            await mediator.Send(new SalvarEncaminhamentoAEECommand(encaminhamentoAEE));
            await ExcluirPendenciasEncaminhamentoAEE(encaminhamentoAEEDto.Situacao, encaminhamentoAEE);
            
            foreach (var secao in encaminhamentoAEEDto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException($"Nenhuma questão foi encontrada na Seção {secao.SecaoId}");

                var secaoExistente = encaminhamentoAEE.Secoes.FirstOrDefault(s => s.SecaoEncaminhamentoAEEId == secao.SecaoId);

                long resultadoEncaminhamentoSecao = 0;
                if (secaoExistente.EhNulo())
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

                    if (questaoExistente.EhNulo())
                    {
                        var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarEncaminhamentoAEESecaoQuestaoCommand(resultadoEncaminhamentoSecao, questoes.FirstOrDefault().QuestaoId));
                        await RegistrarRespostaEncaminhamento(questoes, resultadoEncaminhamentoQuestao);
                    }
                    else
                        await AlterarQuestoesExistentes(questaoExistente, questoes);
                }
                await ExcluirQuestoesExistentes(secaoExistente.Questoes.Where(x => !secao.Questoes.Any(s => s.QuestaoId == x.QuestaoId)));
            }
        }

        private async Task AlterarQuestaoExcluida(QuestaoEncaminhamentoAEE questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarQuestaoEncaminhamentoAEECommand(questao));
        }

        private async Task IncluirRespostasEncaminhamento(QuestaoEncaminhamentoAEE questaoExistente, IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> respostas)
            => await RegistrarRespostaEncaminhamento(ObterRespostasAIncluir(respostas), questaoExistente.Id);

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

        private IEnumerable<EncaminhamentoAEESecaoQuestaoDto> ObterRespostasAIncluir(IGrouping<long, EncaminhamentoAEESecaoQuestaoDto> respostas)
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
            return data.NaoEhNulo() && data.Any();
        }


        private bool EhQuestaoObrigatoriaNaoRespondida(QuestaoDto questao)
        {
            return questao.Obrigatorio &&
                    !NaoNuloEContemRegistros(questao.Resposta);
        }

        private bool QuestaoRespondida(QuestaoDto questao)
        {
            return NaoNuloEContemRegistros(questao.OpcaoResposta)
                   && NaoNuloEContemRegistros(questao.Resposta);
        }

        private string ObterOrdemQuestao(QuestaoDto questao, string questaoPaiOrdem = "")
        {
            return (questaoPaiOrdem != "" ? $"{questaoPaiOrdem}.{questao.Ordem.ToString()}" : questao.Ordem.ToString());
        }
        private void ValidaRecursivo(string secao, string questaoPaiOrdem, IEnumerable<QuestaoDto> questoes, List<dynamic> questoesObrigatoriasNaoRespondidas)
        {
            foreach (var questao in questoes)
            {
                var ordem = ObterOrdemQuestao(questao, questaoPaiOrdem);

                if (EhQuestaoObrigatoriaNaoRespondida(questao))
                    questoesObrigatoriasNaoRespondidas.Add(new { Secao = secao, Ordem = ordem });
                else if (QuestaoRespondida(questao))
                    {
                        foreach (var resposta in questao.Resposta)
                        {
                            var opcao = questao.OpcaoResposta.FirstOrDefault(opcao => opcao.Id == Convert.ToInt64(resposta.Texto));
                            if (opcao?.QuestoesComplementares.Any() ?? false)
                            {
                                ValidaRecursivo(secao, ordem, opcao.QuestoesComplementares, questoesObrigatoriasNaoRespondidas);
                            }
                        }
                    }
            }
        }

        private async Task<IEnumerable<RespostaQuestaoObrigatoriaDto>> ObterRespostasEncaminhamentoAEE(long? encaminhamentoAEEId)
        {
            return encaminhamentoAEEId.HasValue ? (await repositorioQuestaoEncaminhamento.ObterRespostasEncaminhamento(encaminhamentoAEEId.Value))
                 .Select(resposta => new RespostaQuestaoObrigatoriaDto
                 {
                     QuestaoId = resposta.QuestaoId,
                     Resposta = resposta.RespostaId.HasValue ? resposta.RespostaId?.ToString() : resposta.Texto,
                     Persistida = true
                 })
                 : Enumerable.Empty<RespostaQuestaoObrigatoriaDto>();
        }

        private async Task ValidarQuestoesObrigatoriasNaoPreechidas(EncaminhamentoAeeDto encaminhamentoAEEDto)
        {

            List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasNaorespondidas = new List<QuestaoObrigatoriaNaoRespondidaDto>();
            var secoesEtapa = await this.mediator.Send(new ObterSecoesEncaminhamentoDtoPorEtapaQuery(1));
            IEnumerable<RespostaQuestaoObrigatoriaDto> respostasPersistidas = null;

            foreach (var secao in secoesEtapa)
            {
                var secaoPresenteDto = encaminhamentoAEEDto.Secoes.FirstOrDefault(secaoDto => secaoDto.SecaoId == secao.Id);

                IEnumerable<RespostaQuestaoObrigatoriaDto> respostasEncaminhamento;
                if (secaoPresenteDto.NaoEhNulo() && secaoPresenteDto.Questoes.Any())
                {
                    respostasEncaminhamento = secaoPresenteDto.Questoes
                        .Select(questao => new RespostaQuestaoObrigatoriaDto()
                        {
                            QuestaoId = questao.QuestaoId,
                            Resposta = questao.Resposta
                        })
                        .Where(questao => !string.IsNullOrEmpty(questao.Resposta));
                }
                else
                {
                    if (respostasPersistidas.EhNulo())
                        respostasPersistidas = await ObterRespostasEncaminhamentoAEE(encaminhamentoAEEDto.Id);
                    respostasEncaminhamento = respostasPersistidas;
                }


                questoesObrigatoriasNaorespondidas.AddRange(await mediator.Send(new ObterQuestoesObrigatoriasNaoRespondidasQuery(secao, respostasEncaminhamento)));
            }


            if (questoesObrigatoriasNaorespondidas.Any())
            {
                var mensagem = new List<string>();
                foreach (var secao in questoesObrigatoriasNaorespondidas.GroupBy(questao => questao.SecaoNome))
                {
                    mensagem.Add($"Seção: {secao.Key} Questões: [{string.Join(", ", secao.Select(questao => questao.QuestaoOrdem).Distinct().ToArray())}]");
                }
                throw new NegocioException(String.Format(MensagemNegocioEncaminhamentoAee.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS, String.Join(", ", mensagem)));
            }
        }
    }
}


