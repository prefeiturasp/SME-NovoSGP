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
                request.EncaminhamentoId.HasValue
                ? await repositorioQuestaoEncaminhamento.ObterListaPorQuestionario(request.QuestionarioId)
                : await repositorioQuestaoEncaminhamento.ObterListaPorQuestionarioEncaminhamento(request.QuestionarioId, request.EncaminhamentoId);

            var questaoComplementar = dadosQuestionario
                .Where(dq => dq.QuestaoComplementarId.HasValue)
                .Select(dq => dq.QuestaoComplementarId.Value)
                .Distinct()
                .ToArray();

            var questao = dadosQuestionario
                .Where(dq => !questaoComplementar.Contains(dq.QuestaoId))
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

            var opcoesRespostas = questaoLista
                .GroupBy(
                    questaoKey => questaoKey.OpcaoRespostaId,
                    questaoValue => questaoValue,
                    (key, value) =>
                    {
                        var opcaoLista = value.First();
                        var arquivos = value
                            .Where(v => v.RespostaArquivoId.HasValue)
                            .Select(v => new Arquivo
                            {
                                Id = v.RespostaArquivoId.Value,
                                Nome = v.ArquivoNome,
                                Codigo = Guid.Parse(v.ArquivoCodigo),
                                Tipo = (TipoArquivo)v.ArquivoTipo,
                                TipoConteudo = v.ArquivoTipoConteudo
                            })
                            .ToArray();

                        var opcao = new OpcaoRespostaAeeDto
                        {
                            Id = opcaoLista.OpcaoRespostaId,
                            Arquivos = arquivos,
                            Nome = opcaoLista.OpcaoRespostaNome,
                            Ordem = opcaoLista.OpcaoRespostaOrdem,
                            Texto = opcaoLista.RespostaTexto,
                            RespostaEncaminhamentoId = opcaoLista.RespostaEncaminhamentoId,
                            QuestaoComplementar = opcaoLista.QuestaoComplementarId.HasValue
                                ? ObterQuestao(opcaoLista.QuestaoComplementarId.Value, dadosQuestionario)
                                : null
                        };

                        return opcao;
                    }
                )
                .OrderBy(q => q.Ordem)
                .ToArray();

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
                    OpcaoResposta = opcoesRespostas
                };

            return questao;
        }
    }
}
