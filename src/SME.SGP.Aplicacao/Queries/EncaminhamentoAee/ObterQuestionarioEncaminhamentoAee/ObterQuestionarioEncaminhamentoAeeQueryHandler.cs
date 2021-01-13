using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioEncaminhamentoAeeQueryHandler : IRequestHandler<ObterQuestionarioEncaminhamentoAeeQuery, IEnumerable<QuestaoAeeDto>>
    {
        private readonly IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamento;

        public ObterQuestionarioEncaminhamentoAeeQueryHandler(IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamento)
        {
            this.repositorioQuestaoEncaminhamento = repositorioQuestaoEncaminhamento ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamento));
        }


        public async Task<IEnumerable<QuestaoAeeDto>> Handle(ObterQuestionarioEncaminhamentoAeeQuery request, CancellationToken cancellationToken)
        {
            var dadosQuestionario =
                !request.EncaminhamentoId.HasValue
                ? await repositorioQuestaoEncaminhamento.ObterListaPorQuestionario(request.QuestionarioId)
                : await repositorioQuestaoEncaminhamento.ObterListaPorQuestionarioEncaminhamento(request.QuestionarioId, request.EncaminhamentoId);

            var questaoComplementar = dadosQuestionario
                .Where(dq => dq.QuestaoComplementarId.HasValue)
                .Select(dq => dq.QuestaoComplementarId.Value)
                .Distinct()
                .ToArray();

            var questao = dadosQuestionario
                .Where(dq => !questaoComplementar.Contains(dq.QuestaoId))
                .GroupBy(
                    questaoKey => questaoKey.QuestaoId,
                    questaoValue => questaoValue,
                    (key, value) => value.First()
                )               
                .Select(dq => ObterQuestao(dq.QuestaoId, dadosQuestionario))
                .OrderBy(q => q.Ordem)
                .ToArray();

            return questao;
        }

        QuestaoAeeDto ObterQuestao(long questaoId, IEnumerable<QuestaoRespostaAeeDto> dadosQuestionario)
        {
            var questaoLista = dadosQuestionario
                .Where(q => q.QuestaoId == questaoId)
                .ToArray();

            if (!questaoLista.Any()) return null;

            var opcoeResposta = questaoLista
                .Where(or => or.OpcaoRespostaId.HasValue)
                .Where(or => !or.RespostaEncaminhamentoId.HasValue)
                .GroupBy(
                    questaoKey => questaoKey.OpcaoRespostaId.Value,
                    questaoValue => questaoValue,
                    (key, value) =>
                    {
                        var questaoResposta = value.First();

                        var opcao = new OpcaoRespostaAeeDto
                        {
                            Id = questaoResposta.OpcaoRespostaId.Value,
                            Nome = questaoResposta.OpcaoRespostaNome,
                            Ordem = questaoResposta.OpcaoRespostaOrdem.Value,
                            QuestaoComplementar = questaoResposta.QuestaoComplementarId.HasValue
                                ? ObterQuestao(questaoResposta.QuestaoComplementarId.Value, dadosQuestionario)
                                : null
                        };

                        return opcao;
                    }
                )
                .OrderBy(q => q.Ordem)
                .ToArray();

            var respostaLista = questaoLista
                .Where(or => !or.OpcaoRespostaId.HasValue)
                .Where(or => or.RespostaEncaminhamentoId.HasValue)
                .ToArray();

            var respostaArquivos = respostaLista
                .Where(or => or.RespostaArquivoId.HasValue)
                .GroupBy(
                    questaoKey => questaoKey.RespostaArquivoId.Value,
                    questaoValue => questaoValue,
                    (key, value) =>
                    {
                        var questaoResposta = value.First();

                        var arquivo = new Arquivo
                        {
                            Id = questaoResposta.RespostaArquivoId.Value,
                            Nome = questaoResposta.ArquivoNome,
                            Codigo = Guid.Parse(questaoResposta.ArquivoCodigo),
                            Tipo = (TipoArquivo)questaoResposta.ArquivoTipo,
                            TipoConteudo = questaoResposta.ArquivoTipoConteudo
                        };

                        var resposta = new RespostaAeeDto
                        {
                            Id = questaoResposta.RespostaEncaminhamentoId,
                            Arquivo = arquivo,
                            OpcaoRespostaId = null,
                            Texto = null
                        };

                        return resposta;
                    }
                );

            var respostaOpcoes = respostaLista
                .Where(or => or.RespostaEncaminhamentoOpcaoRespostaId.HasValue)
                .GroupBy(
                    questaoKey => questaoKey.RespostaEncaminhamentoOpcaoRespostaId.Value,
                    questaoValue => questaoValue,
                    (key, value) =>
                    {
                        var questaoResposta = value.First();

                        var resposta = new RespostaAeeDto
                        {
                            Id = questaoResposta.RespostaEncaminhamentoId,
                            Arquivo = null,
                            OpcaoRespostaId = questaoResposta.RespostaEncaminhamentoOpcaoRespostaId.Value,
                            Texto = null
                        };

                        return resposta;
                    }
                );

            var respostaTexto = respostaLista
                .Where(or => !string.IsNullOrEmpty(or.RespostaTexto))
                .GroupBy(
                    questaoKey => questaoKey.RespostaTexto,
                    questaoValue => questaoValue,
                    (key, value) =>
                    {
                        var questaoResposta = value.First();

                        var resposta = new RespostaAeeDto
                        {
                            Id = questaoResposta.RespostaEncaminhamentoId,
                            Arquivo = null,
                            OpcaoRespostaId = null,
                            Texto = questaoResposta.RespostaTexto
                        };

                        return resposta;
                    }
                );

            var questao = 
                new QuestaoAeeDto
                {
                    Id = questaoLista.First().QuestaoId,
                    Nome = questaoLista.First().QuestaoNome,
                    Obrigatorio = questaoLista.First().QuestaoObrigatorio,
                    Observacao = questaoLista.First().QuestaoObservacao,
                    Opcionais = questaoLista.First().QuestaoOpcionais,
                    Ordem = questaoLista.First().QuestaoOrder,
                    TipoQuestao = (TipoQuestao)questaoLista.First().QuestaoTipo,
                    OpcaoResposta = opcoeResposta,
                    Resposta = 
                        respostaOpcoes
                        .Union(respostaArquivos)
                        .Union(respostaTexto)
                        .ToArray()
                };

            return questao;
        }
    }
}
