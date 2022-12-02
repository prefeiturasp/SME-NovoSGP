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

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoNAAPAUseCase : IRegistrarEncaminhamentoNAAPAUseCase
    {
        private const string SECAO_QUESTOES_APRESENTADAS = "QUESTOES_APRESENTADAS_INFANTIL";
        private const string QUESTAO_OBSERVACOES_AGRUPAMENTO_PROMOCAO_CUIDADOS = "OBS_AGRUPAMENTO_PROMOCAO_CUIDADOS";
        private const string QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS = "AGRUPAMENTO_PROMOCAO_CUIDADOS";
        private const string QUESTAO_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS = "TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS";
        private const string QUESTAO_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO = "TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO";


        private readonly IMediator mediator;

        public RegistrarEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoEncaminhamentoNAAPADto> Executar(EncaminhamentoNAAPADto encaminhamentoNAAPADto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(encaminhamentoNAAPADto.TurmaId));
            if (turma == null)
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(encaminhamentoNAAPADto.AlunoCodigo, DateTime.Now.Year));
            if (aluno == null)
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            if (!encaminhamentoNAAPADto.Secoes.Any())
                throw new NegocioException(MensagemNegocioComuns.NENHUMA_SECAO_ENCONTRADA);

            if (encaminhamentoNAAPADto.Situacao != SituacaoNAAPA.Rascunho)
                await ValidarQuestoesObrigatoriasNaoPreechidas(encaminhamentoNAAPADto);

            //if  (encaminhamentoNAAPADto.Secoes.Any(s => s.Concluido == false))

                if (encaminhamentoNAAPADto.Id.GetValueOrDefault() > 0)
            {
                var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPADto.Id.GetValueOrDefault()));
                if (encaminhamentoNAAPA != null)
                {
                    await AlterarEncaminhamento(encaminhamentoNAAPADto, encaminhamentoNAAPA);
                    await RemoverArquivosNaoUtilizados(encaminhamentoNAAPADto.Secoes);

                    return new ResultadoEncaminhamentoNAAPADto() { Id = encaminhamentoNAAPA.Id, Auditoria = (AuditoriaDto)encaminhamentoNAAPA};
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
                    var entidadeResposta = await mediator.Send(new ObterRespostaEncaminhamentoNAAPAPorIdQuery(item.RespostaEncaminhamentoId));
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

        private async void ValidaQuestaoObservacaoAgrupamentoPromocaoCuidados(string secao, IEnumerable<QuestaoDto> questoes, List<dynamic> questoesObrigatoriasNaoRespondidas)
        {
            var questaoObservacoesAgrupamentoPromocaoCuidados = questoes.Where(questao => (questao.NomeComponente == QUESTAO_OBSERVACOES_AGRUPAMENTO_PROMOCAO_CUIDADOS) 
                                                                                           && !NaoNuloEContemRegistros(questao.Resposta)).FirstOrDefault();
            if (questaoObservacoesAgrupamentoPromocaoCuidados != null)
            {
                var questaoAgrupamentoPromocaoCuidados = questoes.Where(questao => (questao.NomeComponente == QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS)).FirstOrDefault();
                var questoesComplementares = questaoAgrupamentoPromocaoCuidados.OpcaoResposta.SelectMany(opcao => opcao.QuestoesComplementares)
                                                  .Where(questaoComplementar => questaoComplementar.NomeComponente == QUESTAO_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS
                                                                                || questaoComplementar.NomeComponente == QUESTAO_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO).Distinct();

                var questaoAdoeceComFrequencia = questoesComplementares.Where(questaoComplementar => questaoComplementar.NomeComponente == QUESTAO_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS).FirstOrDefault();
                var questaoDoencaCronica = questoesComplementares.Where(questaoComplementar => questaoComplementar.NomeComponente == QUESTAO_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO).FirstOrDefault();

                if (questaoAdoeceComFrequencia != null)
                {
                    var opcaoOutras_QuestaoAdoeceComFrequencia = (await mediator.Send(new ObterOpcoesRespostaPorQuestaoIdQuery(questaoAdoeceComFrequencia.Id))).Where(opcao => opcao.Nome == "Outras").FirstOrDefault();
                    if (questaoAdoeceComFrequencia.OpcaoResposta.Any(opcao => opcao.Id == opcaoOutras_QuestaoAdoeceComFrequencia.Id))
                        questoesObrigatoriasNaoRespondidas.Add(new { Secao = secao, Ordem = questaoObservacoesAgrupamentoPromocaoCuidados.Ordem });
                }
                if (questaoDoencaCronica != null)
                { 
                    var opcaoOutras_QuestaoDoencaCronica = (await mediator.Send(new ObterOpcoesRespostaPorQuestaoIdQuery(questaoDoencaCronica.Id))).Where(opcao => opcao.Nome == "Outras").FirstOrDefault();
                    if (questaoDoencaCronica.OpcaoResposta.Any(opcao => opcao.Id == opcaoOutras_QuestaoDoencaCronica.Id))
                        questoesObrigatoriasNaoRespondidas.Add(new { Secao = secao, Ordem = questaoObservacoesAgrupamentoPromocaoCuidados.Ordem });
                }
                
            }
        }

        private async Task ValidarQuestoesObrigatoriasNaoPreechidas(EncaminhamentoNAAPADto encaminhamentoNAAPADto)
        {

            var secoesEtapa = await this.mediator.Send(new ObterSecoesEncaminhamentoNAAPADtoPorEtapaQuery(1));
            var questoesObrigatoriasNaorespondidas = new List<dynamic>();
            var respostasEncaminhamento = Enumerable.Empty<EncaminhamentoNAAPASecaoQuestaoDto>();

            var respostasPersistidas = (encaminhamentoNAAPADto.Id.HasValue) ? (await mediator.Send(new ObterQuestaoRespostaEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPADto.Id.Value)))
                                                                                .Select(resposta => new EncaminhamentoNAAPASecaoQuestaoDto()
                                                                                {
                                                                                    QuestaoId = resposta.QuestaoId,
                                                                                    Resposta = (resposta.RespostaId ?? 0) != 0 ? resposta.RespostaId.Value.ToString() : resposta.Texto,
                                                                                    RespostaEncaminhamentoId = resposta.Id
                                                                                }) : Enumerable.Empty<EncaminhamentoNAAPASecaoQuestaoDto>();

            foreach (var secao in secoesEtapa)
            {
                var secaoPresenteDto = encaminhamentoNAAPADto.Secoes.Where(w => secoesEtapa.Any(s => s.Id == w.SecaoId)).FirstOrDefault();
                if (secaoPresenteDto != null && secaoPresenteDto.Questoes.Any())
                {
                    respostasEncaminhamento = secaoPresenteDto.Questoes
                                                .Select(questao => new EncaminhamentoNAAPASecaoQuestaoDto()
                                                                    {
                                                                        QuestaoId = questao.QuestaoId,
                                                                        Resposta = questao.Resposta,
                                                                        RespostaEncaminhamentoId = questao.RespostaEncaminhamentoId
                                                                    })
                                                .Where(questao => !string.IsNullOrEmpty(questao.Resposta));
                }
                else respostasEncaminhamento = respostasPersistidas;

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

                if (secao.NomeComponente == SECAO_QUESTOES_APRESENTADAS)
                    ValidaQuestaoObservacaoAgrupamentoPromocaoCuidados(secao.Nome, questoes, questoesObrigatoriasNaorespondidas);
                
                if (!questoes.Any(questao => questao.Obrigatorio)) { continue; }
                ValidaRecursivo(secao.Nome, "", questoes, questoesObrigatoriasNaorespondidas);


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


