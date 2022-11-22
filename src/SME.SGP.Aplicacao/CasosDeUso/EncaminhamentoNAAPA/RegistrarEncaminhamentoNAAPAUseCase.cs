using Dapper;
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
    public class RegistrarEncaminhamentoNAAPAUseCase : IRegistrarEncaminhamentoNAAPAUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaEncaminhamentoNAAPA repositorioRespostaEncaminhamentoNAAPA;
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA;

        public RegistrarEncaminhamentoNAAPAUseCase(IMediator mediator, IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA, IRepositorioRespostaEncaminhamentoNAAPA repositorioRespostaEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoNAAPA = repositorioRespostaEncaminhamentoNAAPA ?? throw new System.ArgumentNullException(nameof(repositorioRespostaEncaminhamentoNAAPA));
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<ResultadoEncaminhamentoNAAPADto> Executar(EncaminhamentoNAAPADto encaminhamentoNAAPADto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoNAAPADto.TurmaId));
            if (turma == null)
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(encaminhamentoNAAPADto.AlunoCodigo, DateTime.Now.Year));
            if (aluno == null)
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            var alunoEncaminhamentoNAAPA = await mediator.Send(new ExisteEncaminhamentoNAAPAPorEstudanteQuery(encaminhamentoNAAPADto.AlunoCodigo, turma.Ue.Id));
            if (alunoEncaminhamentoNAAPA && encaminhamentoNAAPADto.Id == 0)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ESTUDANTE_JA_POSSUI_ENCAMINHAMENTO_NAAPA_EM_ABERTO);

            if (!encaminhamentoNAAPADto.Secoes.Any())
                throw new NegocioException(MensagemNegocioComuns.NENHUMA_SECAO_ENCONTRADA);

            if(encaminhamentoNAAPADto.Secoes.Any(s => s.Concluido == false))
                await ValidarQuestoesObrigatoriasNaoPreechidas(encaminhamentoNAAPADto);

            if (encaminhamentoNAAPADto.Id.GetValueOrDefault() > 0)
            {
                var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPADto.Id.GetValueOrDefault()));
                if (encaminhamentoNAAPA != null)
                {
                    await AlterarEncaminhamento(encaminhamentoNAAPADto, encaminhamentoNAAPA);
                    await RemoverArquivosNaoUtilizados(encaminhamentoNAAPADto.Secoes);

                    return new ResultadoEncaminhamentoNAAPADto() { Id = encaminhamentoNAAPA.Id };
                }
            }

            var resultadoEncaminhamento = await mediator.Send(new RegistrarEncaminhamentoNAAPACommand(
            encaminhamentoNAAPADto.TurmaId, aluno.NomeAluno, aluno.CodigoAluno,
            encaminhamentoNAAPADto.Situacao));

            await SalvarEncaminhamento(encaminhamentoNAAPADto, resultadoEncaminhamento);

            return resultadoEncaminhamento;
        }
      
        private async Task RemoverArquivosNaoUtilizados(List<EncaminhamentoNAAPASecaoDto> secoes)
        {
            var resposta = new List<EncaminhamentoNAAPASecaoQuestaoDto>();
            foreach (var s in secoes)
            {
                foreach (var q in s.Questoes)
                {
                    if (string.IsNullOrEmpty(q.Resposta) && q.TipoQuestao == TipoQuestao.Upload)
                        resposta.Add(q);
                }
            }

            if (resposta != null && resposta.Any())
            {
                foreach (var item in resposta)
                {
                    var entidadeResposta = repositorioRespostaEncaminhamentoNAAPA.ObterPorId(item.RespostaEncaminhamentoId);
                    if (entidadeResposta != null)
                        await mediator.Send(new ExcluirRespostaEncaminhamentoNAAPACommand(entidadeResposta));
                }
            }
        }
        public async Task AlterarEncaminhamento(EncaminhamentoNAAPADto encaminhamentoNAAPADto, EncaminhamentoNAAPA encaminhamentoNAAPA)
        {
            encaminhamentoNAAPA.Situacao = encaminhamentoNAAPADto.Situacao;
            await mediator.Send(new SalvarEncaminhamentoNAAPACommand(encaminhamentoNAAPA));

            foreach (var secao in encaminhamentoNAAPADto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X,secao.SecaoId));

                var secaoExistente = encaminhamentoNAAPA.Secoes.FirstOrDefault(s => s.SecaoEncaminhamentoNAAPAId == secao.SecaoId);

                long resultadoEncaminhamentoSecao = 0;
                if (secaoExistente == null)
                    secaoExistente = await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoCommand(encaminhamentoNAAPA.Id, secao.SecaoId, secao.Concluido));
                else
                {
                    secaoExistente.Concluido = secao.Concluido;
                    await mediator.Send(new AlterarEncaminhamentoNAAPASecaoCommand(secaoExistente));
                }

                resultadoEncaminhamentoSecao = secaoExistente.Id;

                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var questaoExistente = secaoExistente.Questoes.FirstOrDefault(q => q.QuestaoId == questoes.FirstOrDefault().QuestaoId);

                    if (questaoExistente == null)
                    {
                        var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoQuestaoCommand(resultadoEncaminhamentoSecao, questoes.FirstOrDefault().QuestaoId));
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
                    await mediator.Send(new ExcluirQuestaoEncaminhamentoNAAPAPorIdCommand(questao.Id));
            }
        }

        private async Task AlterarQuestaoExcluida(QuestaoEncaminhamentoNAAPA questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarQuestaoEncaminhamentoNAAPACommand(questao));
        }

        private async Task IncluirRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas)
            => await RegistrarRespostaEncaminhamento(ObterRespostasAIncluir(questaoExistente, respostas), questaoExistente.Id);

        private async Task RegistrarRespostaEncaminhamento(IEnumerable<EncaminhamentoNAAPASecaoQuestaoDto> questoes, long questaoEncaminhamentoId)
        {
            foreach (var questao in questoes)
            {
                await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(questao.Resposta, questaoEncaminhamentoId, questao.TipoQuestao));
            }
        }

        private async Task AlterarRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas)
        {
            foreach (var respostaAlterar in ObterRespostasAAlterar(questaoExistente, respostas))
                await mediator.Send(new AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(respostaAlterar, respostas.FirstOrDefault(c => c.RespostaEncaminhamentoId == respostaAlterar.Id)));
        }

        private async Task ExcluirRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questoesExistentes, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas)
        {
            foreach (var respostasExcluir in ObterRespostasAExcluir(questoesExistentes, respostas))
                await mediator.Send(new ExcluirRespostaEncaminhamentoNAAPACommand(respostasExcluir));
        }

        private IEnumerable<EncaminhamentoNAAPASecaoQuestaoDto> ObterRespostasAIncluir(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas)
            => respostas.Where(c => c.RespostaEncaminhamentoId == 0);

        private IEnumerable<RespostaEncaminhamentoNAAPA> ObterRespostasAExcluir(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
        {
            var retorno = questaoExistente.Respostas.Where(s => !respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));
            return retorno;
        }

        private IEnumerable<RespostaEncaminhamentoNAAPA> ObterRespostasAAlterar(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
            => questaoExistente.Respostas.Where(s => respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));

        public async Task SalvarEncaminhamento(EncaminhamentoNAAPADto encaminhamentoNAAPADto, ResultadoEncaminhamentoNAAPADto resultadoEncaminhamento)
        {
            foreach (var secao in encaminhamentoNAAPADto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X,secao.SecaoId));

                var secaoEncaminhamento = await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoCommand(resultadoEncaminhamento.Id, secao.SecaoId, secao.Concluido));

                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoQuestaoCommand(secaoEncaminhamento.Id, questoes.FirstOrDefault().QuestaoId));
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

        private async Task ValidarQuestoesObrigatoriasNaoPreechidas(EncaminhamentoNAAPADto encaminhamentoNAAPADto)
        {
            
            var secoesEtapa = await this.mediator.Send(new ObterSecoesEncaminhamentoDtoPorEtapaQuery(1));
            var contemSecaoNaResposta = encaminhamentoNAAPADto.Secoes.Where(w => secoesEtapa.Any(s => s.Id == w.SecaoId));
            var questoesObrigatoriasNaorespondidas = new List<dynamic>();

            if (contemSecaoNaResposta.Any())
            {
                var respostasEncaminhamento = encaminhamentoNAAPADto.Secoes.Where(sessao => sessao.Questoes.Any())
                    .SelectMany(secao => secao.Questoes,
                                (secao, questao) => new { questao.QuestaoId, questao.Resposta, questao.RespostaEncaminhamentoId })
                    .Where(questao => !string.IsNullOrEmpty(questao.Resposta));
                foreach (var secao in secoesEtapa)
                {
                    var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secao.QuestionarioId, questaoId =>
                        respostasEncaminhamento.Where(c => c.QuestaoId == questaoId)                        
                        .Select(respostasEncaminhamento =>
                        {
                            return new RespostaQuestaoDto()
                            {
                                Id = GetHashCode(),
                                OpcaoRespostaId = 0,
                                Texto = respostasEncaminhamento.Resposta,
                                Arquivo = null
                            };
                        })));

                    if (!questoes.Any(questao => questao.Obrigatorio)) { continue; }
                    ValidaRecursivo(secao.Nome, "", questoes, questoesObrigatoriasNaorespondidas);
                }                
            }
            else
            {
                var respostasJaPreenchidas = encaminhamentoNAAPADto.Id.HasValue ?
                await repositorioQuestaoEncaminhamentoNAAPA.ObterRespostasEncaminhamento(encaminhamentoNAAPADto.Id.Value) :
                Enumerable.Empty<RespostaQuestaoEncaminhamentoNAAPADto>();

                if (encaminhamentoNAAPADto.Situacao != SituacaoNAAPA.Rascunho)
                {
                    foreach (var secao in secoesEtapa)
                    {
                        var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secao.QuestionarioId, questaoId =>
                            respostasJaPreenchidas.Where(c => c.QuestaoId == questaoId)
                            .Select(respostasJaPreenchidas =>
                            {
                                return new RespostaQuestaoDto()
                                {
                                    Id = GetHashCode(),
                                    OpcaoRespostaId = 0,
                                    Texto = respostasJaPreenchidas.Texto,
                                    Arquivo = null
                                };
                            })));

                        if (!questoes.Any(questao => questao.Obrigatorio)) { continue; }
                        ValidaRecursivo(secao.Nome, "", questoes, questoesObrigatoriasNaorespondidas);
                    }                    
                }
            }
            if (questoesObrigatoriasNaorespondidas.Any() && encaminhamentoNAAPADto.Situacao != SituacaoNAAPA.Rascunho)
            {
                var mensagem = new List<string>();
                foreach (var secao in questoesObrigatoriasNaorespondidas.GroupBy(questao => questao.Secao))
                {
                    mensagem.Add($"Seção: {secao.Key} Questões: [{string.Join(", ", secao.Select(questao => questao.Ordem).Distinct().ToArray())}]");
                }
                throw new NegocioException(String.Format(MensagemNegocioEncaminhamentoNAAPA.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS, String.Join(", ", mensagem)));
            }
        }
    }
}


