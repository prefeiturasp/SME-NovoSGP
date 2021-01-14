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
            var dadosQuestionario = await repositorioQuestaoEncaminhamento.ObterListaPorQuestionario(request.QuestionarioId);

            var questoesComplementares = dadosQuestionario
                .Where(dq => dq.OpcoesRespostas.Any(a => a.QuestaoComplementarId.HasValue))
                .SelectMany(dq => dq.OpcoesRespostas.Where(c => c.QuestaoComplementarId.HasValue).Select(a => a.QuestaoComplementarId))
                .Distinct();

            var respostasEncaminhamento = request.EncaminhamentoId.HasValue ?
                await repositorioQuestaoEncaminhamento.ObterRespostasEncaminhamento(request.EncaminhamentoId.Value) :
                Enumerable.Empty<RespostaQuestaoEncaminhamentoAEEDto>();

            var questao = dadosQuestionario
                .Where(dq => !questoesComplementares.Contains(dq.Id))
                .Select(dq => ObterQuestao(dq.Id, dadosQuestionario, respostasEncaminhamento))
                .OrderBy(q => q.Ordem)
                .ToArray();

            return questao;
        }

        QuestaoAeeDto ObterQuestao(long questaoId, IEnumerable<Questao> dadosQuestionario, IEnumerable<RespostaQuestaoEncaminhamentoAEEDto> respostasEncaminhamento)
        {
            var questao = dadosQuestionario.FirstOrDefault(c => c.Id == questaoId);

            return new QuestaoAeeDto()
            {
                Id = questao.Id,
                Ordem = questao.Ordem,
                Nome = questao.Nome,
                TipoQuestao = questao.TipoQuestao,
                Obrigatorio = questao.Obrigatorio,
                Observacao = questao.Observacao,
                Opcionais = questao.Opcionais,
                OpcaoResposta = questao.OpcoesRespostas.Select(opcaoResposta =>
                {
                    return new OpcaoRespostaAeeDto()
                    {
                        Id = opcaoResposta.Id,
                        Nome = opcaoResposta.Nome,
                        Ordem = opcaoResposta.Ordem,
                        QuestaoComplementar = opcaoResposta.QuestaoComplementarId.HasValue ?
                            ObterQuestao(opcaoResposta.QuestaoComplementarId.Value, dadosQuestionario, respostasEncaminhamento) :
                            null
                    };
                }).ToArray(),
                Resposta = respostasEncaminhamento.Where(c => c.QuestaoId == questaoId).Select(respostaEncaminhamento =>
                {
                    return new RespostaAeeDto()
                    {
                        Id = respostaEncaminhamento.Id,
                        OpcaoRespostaId = respostaEncaminhamento.RespostaId,
                        Texto = respostaEncaminhamento.Texto,
                        Arquivo = respostaEncaminhamento.Arquivo
                    };
                }).ToArray()
            };

        }
    }
}
