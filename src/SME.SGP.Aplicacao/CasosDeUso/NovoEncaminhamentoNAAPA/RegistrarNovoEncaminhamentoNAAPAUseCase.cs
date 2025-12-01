using MediatR;
using SME.SGP.Aplicacao.Commands.EncaminhamentoNAAPA.ExcluirQuestaoNovoEncaminhamentoNAAPAPorId;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.AlterarNovoEncaminhamentoNAAPASecaoQuestaoResposta;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.AlterarQuestaoEncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.ExcluirRespostaNovoEncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.NovoEncaminhamentoNAAPASecao;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPASecao;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPASecaoQuestao;
using SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPASecaoQuestaoResposta;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterNovoEncaminhamentoNAAPAPorId;
using SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterQuestaoRespostaEncaminhamentoNAAPAPorId;
using SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterRespostaNovoEncaminhamentoNAAPAPorId;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.NovoEncaminhamentoNAAPA
{
    public class RegistrarNovoEncaminhamentoNAAPAUseCase : IRegistrarNovoEncaminhamentoNAAPAUseCase
    {
        private const string SECAO_QUESTOES_APRESENTADAS = "QUESTOES_APRESENTADAS_INFANTIL";
        private const string QUESTAO_OBSERVACOES_AGRUPAMENTO_PROMOCAO_CUIDADOS = "OBS_AGRUPAMENTO_PROMOCAO_CUIDADOS";
        private const string QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS = "AGRUPAMENTO_PROMOCAO_CUIDADOS";
        private const string QUESTAO_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS = "TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS";
        private const string QUESTAO_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO = "TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO";

        private readonly IMediator mediator;

        public RegistrarNovoEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }



        public async Task<ResultadoNovoEncaminhamentoNAAPADto> Executar(NovoEncaminhamentoNAAPADto encaminhamentoNAAPADto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(encaminhamentoNAAPADto.TurmaId));
            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(encaminhamentoNAAPADto.AlunoCodigo, DateTime.Now.Year));
            if (aluno.EhNulo())
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir = await ObterQuestoesObrigatoriasNaoPreechidas(encaminhamentoNAAPADto, (int)turma.ModalidadeCodigo);

            if (questoesObrigatoriasAConsistir.Any() && encaminhamentoNAAPADto.Situacao != SituacaoNAAPA.Rascunho)
            {
                var mensagem = questoesObrigatoriasAConsistir.GroupBy(questao => questao.SecaoNome).Select(secao =>
                        $"Seção: {secao.Key} Questões: [{string.Join(", ", secao.Select(questao => questao.QuestaoOrdem).Distinct().ToArray())}]")
                    .ToList();

                throw new NegocioException(string.Format(
                    MensagemNegocioNovoEncaminhamentoNAAPA.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                    string.Join(", ", mensagem)));
            }

            var secoesComQuestoesObrigatoriasAConsistir = questoesObrigatoriasAConsistir
                .Select(questao => questao.SecaoId).Distinct().ToArray();

            foreach (var secao in encaminhamentoNAAPADto.Secoes)
                secao.Concluido = !secoesComQuestoesObrigatoriasAConsistir.Contains(secao.SecaoId);

            if (encaminhamentoNAAPADto.Id.GetValueOrDefault() > 0)
            {
                var encaminhamentoNAAPA = await mediator.Send(new ObterNovoEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPADto.Id.GetValueOrDefault()));

                if (encaminhamentoNAAPA.NaoEhNulo())
                {
                    await AlterarEncaminhamento(encaminhamentoNAAPADto, encaminhamentoNAAPA);
                    await RemoverArquivosNaoUtilizados(encaminhamentoNAAPADto.Secoes);

                    return new ResultadoNovoEncaminhamentoNAAPADto
                    { Id = encaminhamentoNAAPA.Id, Auditoria = (AuditoriaDto)encaminhamentoNAAPA };
                }
            }

            var resultadoEncaminhamento = await mediator.Send(new RegistrarNovoEncaminhamentoNAAPACommand(
                encaminhamentoNAAPADto.TurmaId, aluno.NomeAluno, aluno.CodigoAluno,
                encaminhamentoNAAPADto.Situacao));

            await SalvarEncaminhamento(encaminhamentoNAAPADto, resultadoEncaminhamento);

            return resultadoEncaminhamento;
        }

        private async Task RemoverArquivosNaoUtilizados(List<NovoEncaminhamentoNAAPASecaoDto> secoes)
        {
            var resposta = new List<NovoEncaminhamentoNAAPASecaoQuestaoDto>();

            foreach (var s in secoes)
            {
                foreach (var q in s.Questoes)
                {
                    if (string.IsNullOrEmpty(q.Resposta) && q.TipoQuestao == TipoQuestao.Upload)
                        resposta.Add(q);
                }
            }

            if (resposta.Any())
            {
                foreach (var item in resposta)
                {
                    var entidadeResposta = await mediator.Send(new ObterRespostaNovoEncaminhamentoNAAPAPorIdQuery(item.RespostaEncaminhamentoId));
                    if (entidadeResposta.NaoEhNulo())
                        await mediator.Send(new ExcluirRespostaNovoEncaminhamentoNAAPACommand(entidadeResposta));
                }
            }
        }
        public async Task AlterarEncaminhamento(NovoEncaminhamentoNAAPADto encaminhamentoNAAPADto, SME.SGP.Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA)
        {
            if (encaminhamentoNAAPADto.Situacao != encaminhamentoNAAPA.Situacao)
                await mediator.Send(new AlterarSituacaoNAAPACommand(encaminhamentoNAAPA, encaminhamentoNAAPADto.Situacao));

            foreach (var secao in encaminhamentoNAAPADto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X, secao.SecaoId));

                var secaoExistente = encaminhamentoNAAPA.Secoes.FirstOrDefault(s => s.SecaoEncaminhamentoNAAPAId == secao.SecaoId);
                var tipoHistorico = TipoHistoricoAlteracoesEncaminhamentoNAAPA.Alteracao;

                if (secaoExistente.EhNulo())
                {
                    secaoExistente = await mediator.Send(new RegistrarNovoEncaminhamentoNAAPASecaoCommand(encaminhamentoNAAPA.Id, secao.SecaoId, secao.Concluido));
                    tipoHistorico = TipoHistoricoAlteracoesEncaminhamentoNAAPA.Inserido;
                }
                else
                {
                    secaoExistente.Concluido = secao.Concluido;
                    await mediator.Send(new AlterarNovoEncaminhamentoNAAPASecaoCommand(secaoExistente));
                }

                var resultadoEncaminhamentoSecao = secaoExistente.Id;

                await mediator.Send(new RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPACommand(secao, secaoExistente, tipoHistorico));

                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var questaoExistente = secaoExistente.Questoes.FirstOrDefault(q => q.QuestaoId == questoes.FirstOrDefault().QuestaoId);

                    if (questaoExistente.EhNulo())
                    {
                        var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommand(resultadoEncaminhamentoSecao, questoes.FirstOrDefault().QuestaoId));
                        await RegistrarRespostaEncaminhamento(questoes, resultadoEncaminhamentoQuestao);
                    }
                    else
                        await AlterarQuestoesExistentes(questaoExistente, questoes);
                }
                await ExcluirQuestoesExistentes(secaoExistente.Questoes.Where(x => !secao.Questoes.Any(s => s.QuestaoId == x.QuestaoId)));
            }
        }

        private async Task ExcluirQuestoesExistentes(IEnumerable<QuestaoEncaminhamentoNAAPA> questoesRemovidas)
        {
            foreach (var questao in questoesRemovidas)
                await mediator.Send(new ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommand(questao.Id));
        }

        private async Task AlterarQuestoesExistentes(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, NovoEncaminhamentoNAAPASecaoQuestaoDto> questoesRespostas)
        {
            if (questaoExistente.Excluido)
                await AlterarQuestaoExcluida(questaoExistente);
            await ExcluirRespostasEncaminhamento(questaoExistente, questoesRespostas);
            await AlterarRespostasEncaminhamento(questaoExistente, questoesRespostas);
            await IncluirRespostasEncaminhamento(questaoExistente, questoesRespostas);
        }

        private async Task AlterarQuestaoExcluida(QuestaoEncaminhamentoNAAPA questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarQuestaoNovoEncaminhamentoNAAPACommand(questao));
        }

        private async Task IncluirRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, NovoEncaminhamentoNAAPASecaoQuestaoDto> respostas)
            => await RegistrarRespostaEncaminhamento(ObterRespostasAIncluir(respostas), questaoExistente.Id);

        private async Task RegistrarRespostaEncaminhamento(IEnumerable<NovoEncaminhamentoNAAPASecaoQuestaoDto> questoes, long questaoEncaminhamentoId)
        {
            foreach (var questao in questoes)
            {
                await mediator.Send(new RegistrarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand(questao.Resposta, questaoEncaminhamentoId, questao.TipoQuestao));
            }
        }

        private async Task AlterarRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, NovoEncaminhamentoNAAPASecaoQuestaoDto> respostas)
        {
            foreach (var respostaAlterar in ObterRespostasAAlterar(questaoExistente, respostas))
                await mediator.Send(new AlterarNovoEncaminhamentoNAAPASecaoQuestaoRespostaCommand(respostaAlterar, respostas.FirstOrDefault(c => c.RespostaEncaminhamentoId == respostaAlterar.Id)));
        }

        private async Task ExcluirRespostasEncaminhamento(QuestaoEncaminhamentoNAAPA questoesExistentes, IGrouping<long, NovoEncaminhamentoNAAPASecaoQuestaoDto> respostas)
        {
            foreach (var respostasExcluir in ObterRespostasAExcluir(questoesExistentes, respostas))
                await mediator.Send(new ExcluirRespostaEncaminhamentoNAAPACommand(respostasExcluir));
        }

        private IEnumerable<NovoEncaminhamentoNAAPASecaoQuestaoDto> ObterRespostasAIncluir(IGrouping<long, NovoEncaminhamentoNAAPASecaoQuestaoDto> respostas)
            => respostas.Where(c => c.RespostaEncaminhamentoId == 0);

        private IEnumerable<RespostaEncaminhamentoNAAPA> ObterRespostasAExcluir(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, NovoEncaminhamentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
        {
            var retorno = questaoExistente.Respostas.Where(s => !respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));
            return retorno;
        }

        private IEnumerable<RespostaEncaminhamentoNAAPA> ObterRespostasAAlterar(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, NovoEncaminhamentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
            => questaoExistente.Respostas.Where(s => respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));

        public async Task SalvarEncaminhamento(NovoEncaminhamentoNAAPADto encaminhamentoNAAPADto, ResultadoNovoEncaminhamentoNAAPADto resultadoEncaminhamento)
        {
            foreach (var secao in encaminhamentoNAAPADto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X, secao.SecaoId));

                var secaoEncaminhamento = await mediator.Send(new RegistrarNovoEncaminhamentoNAAPASecaoCommand(resultadoEncaminhamento.Id, secao.SecaoId, secao.Concluido));

                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var resultadoEncaminhamentoQuestao = await mediator.Send(new RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommand(secaoEncaminhamento.Id, questoes.FirstOrDefault().QuestaoId));
                    await RegistrarRespostaEncaminhamento(questoes, resultadoEncaminhamentoQuestao);
                }

                await mediator.Send(new RegistrarHistoricoDeAlteracaoNovoEncaminhamentoNAAPACommand(secao, secaoEncaminhamento, TipoHistoricoAlteracoesEncaminhamentoNAAPA.Inserido));
            }
        }

        private async Task ValidaQuestaoObservacaoAgrupamentoPromocaoCuidados(SecaoQuestionarioDto secaoAValidar, IEnumerable<QuestaoDto> questoes,
                                                                              List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasNaoRespondidas)
        {
            if (secaoAValidar.NomeComponente != SECAO_QUESTOES_APRESENTADAS)
                return;

            var questaoObservacoes = ObterQuestaoObservacoesAgrupamentoPromocaoCuidadosNaoPreenchida(questoes);
            if (questaoObservacoes.NaoEhNulo())
            {
                var questaoAgrupamentoPromocaoCuidados = ObterQuestaoAgrupamentoPromocaoCuidados(questoes);
                var questoesComplementares = ObterQuestoesComplementaresAgrupamentoPromocaoCuidados(questaoAgrupamentoPromocaoCuidados);
                var questaoAdoeceComFrequencia = ObterQuestaoComplementarTipoAdoeceComFrequenciaSemCuidadosMedicos(questoesComplementares);
                var questaoDoencaCronica = ObterQuestaoComplementarTipoDoencaCronicaTratamentoLongaDuracao(questoesComplementares);

                if (questaoAdoeceComFrequencia.NaoEhNulo())
                {
                    var opcaoOutras_QuestaoAdoeceComFrequencia = (await mediator.Send(new ObterOpcoesRespostaPorQuestaoIdQuery(questaoAdoeceComFrequencia.Id))).FirstOrDefault(opcao => opcao.Nome == "Outras");
                    if (questaoAdoeceComFrequencia.Resposta.Any(resposta => resposta.Texto == opcaoOutras_QuestaoAdoeceComFrequencia.Id.ToString()))
                        questoesObrigatoriasNaoRespondidas.Add(new QuestaoObrigatoriaNaoRespondidaDto(secaoAValidar.Id, secaoAValidar.Nome, questaoObservacoes.Ordem.ToString()));
                }

                if (questaoDoencaCronica.NaoEhNulo())
                {
                    var opcaoOutras_QuestaoDoencaCronica = (await mediator.Send(new ObterOpcoesRespostaPorQuestaoIdQuery(questaoDoencaCronica.Id))).FirstOrDefault(opcao => opcao.Nome == "Outras");
                    if (questaoDoencaCronica.Resposta.Any(resposta => resposta.Texto == opcaoOutras_QuestaoDoencaCronica.Id.ToString()))
                        questoesObrigatoriasNaoRespondidas.Add(new QuestaoObrigatoriaNaoRespondidaDto(secaoAValidar.Id, secaoAValidar.Nome, questaoObservacoes.Ordem.ToString()));
                }
            }
        }

        private QuestaoDto ObterQuestaoObservacoesAgrupamentoPromocaoCuidadosNaoPreenchida(IEnumerable<QuestaoDto> questoes)
        {
            return questoes.FirstOrDefault(questao => (questao.NomeComponente == QUESTAO_OBSERVACOES_AGRUPAMENTO_PROMOCAO_CUIDADOS)
                                              && !questao.Resposta.NaoNuloEContemRegistrosRespondidos());
        }

        private QuestaoDto ObterQuestaoAgrupamentoPromocaoCuidados(IEnumerable<QuestaoDto> questoes)
        {
            return questoes.FirstOrDefault(questao => questao.NomeComponente == QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS);
        }

        private IEnumerable<QuestaoDto> ObterQuestoesComplementaresAgrupamentoPromocaoCuidados(QuestaoDto questaoAgrupamentoPromocaoCuidados)
        {
            return questaoAgrupamentoPromocaoCuidados.OpcaoResposta.SelectMany(opcao => opcao.QuestoesComplementares)
                                                  .Where(questaoComplementar => questaoComplementar.NomeComponente == QUESTAO_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS
                                                                                || questaoComplementar.NomeComponente == QUESTAO_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO).Distinct();
        }

        private QuestaoDto ObterQuestaoComplementarTipoAdoeceComFrequenciaSemCuidadosMedicos(IEnumerable<QuestaoDto> questoesComplementares)
        {
            return questoesComplementares.FirstOrDefault(questaoComplementar => questaoComplementar.NomeComponente == QUESTAO_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS);
        }

        private QuestaoDto ObterQuestaoComplementarTipoDoencaCronicaTratamentoLongaDuracao(IEnumerable<QuestaoDto> questoesComplementares)
        {
            return questoesComplementares.FirstOrDefault(questaoComplementar => questaoComplementar.NomeComponente == QUESTAO_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO);
        }

        private async Task<IEnumerable<RespostaQuestaoObrigatoriaDto>> ObterRespostasEncaminhamentoNAAPA(long? encaminhamentoNAAPAId)
        {
            return encaminhamentoNAAPAId.HasValue ? (await mediator.Send(
                    new ObterQuestaoRespostaNovoEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPAId.Value)))
                .Select(resposta => new RespostaQuestaoObrigatoriaDto
                {
                    QuestaoId = resposta.QuestaoId,
                    Resposta = resposta.RespostaId.HasValue ? resposta.RespostaId?.ToString() : resposta.Texto,
                    Persistida = true
                })
                : Enumerable.Empty<RespostaQuestaoObrigatoriaDto>();
        }
        private async Task<List<QuestaoObrigatoriaNaoRespondidaDto>> ObterQuestoesObrigatoriasNaoPreechidas(NovoEncaminhamentoNAAPADto encaminhamentoNAAPADto, int codigoModalidade)

        {
            List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir = new List<QuestaoObrigatoriaNaoRespondidaDto>();            
            var secoesEtapa = await mediator.Send(new ObterSecaoEncaminhamentoIndividualQuery(null));
            IEnumerable<RespostaQuestaoObrigatoriaDto> respostasPersistidas = null;

            foreach (var secao in secoesEtapa)
            {
                var secaoPresenteDto = encaminhamentoNAAPADto.Secoes.FirstOrDefault(secaoDto => secaoDto.SecaoId == secao.Id);

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
                        respostasPersistidas = await ObterRespostasEncaminhamentoNAAPA(encaminhamentoNAAPADto.Id);
                    respostasEncaminhamento = respostasPersistidas;
                }


                questoesObrigatoriasAConsistir.AddRange(await mediator.Send(new ObterQuestoesObrigatoriasNaoRespondidasQuery(secao, respostasEncaminhamento, ValidaQuestaoObservacaoAgrupamentoPromocaoCuidados)));
            }

            return questoesObrigatoriasAConsistir;
        }
    }
}