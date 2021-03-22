using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
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
    public delegate IEnumerable<RespostaQuestaoDto> ObterRespostasFunc(long questaoId);

    public class ObterQuestoesPorQuestionarioPorIdQueryHandler : IRequestHandler<ObterQuestoesPorQuestionarioPorIdQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IRepositorioQuestionario repositorioQuestionario;

        public ObterQuestoesPorQuestionarioPorIdQueryHandler(IRepositorioQuestionario repositorioQuestionario)
        {
            this.repositorioQuestionario = repositorioQuestionario ?? throw new ArgumentNullException(nameof(repositorioQuestionario));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestoesPorQuestionarioPorIdQuery request, CancellationToken cancellationToken)
        {
            var dadosQuestionario = await repositorioQuestionario.ObterQuestoesPorQuestionarioId(request.QuestionarioId);

            var questoesComplementares = dadosQuestionario
                .Where(dq => dq.OpcoesRespostas.Any(a => a.QuestoesComplementares.Any()))
                .SelectMany(dq => dq.OpcoesRespostas.Where(c => c.QuestoesComplementares.Any()).SelectMany(a => a.QuestoesComplementares.Select(q => q.QuestaoComplementarId)))
                .Distinct();

            var questoes = dadosQuestionario
                .Where(dq => !questoesComplementares.Contains(dq.Id))
                .Select(dq => ObterQuestao(dq.Id, dadosQuestionario, request.ObterRespostas))
                .OrderBy(q => q.Ordem)
                .ToArray();

            return questoes;
        }

        private IEnumerable<QuestaoDto> ObterQuestoes(List<OpcaoQuestaoComplementar> questoesComplementares, IEnumerable<Questao> dadosQuestionario, ObterRespostasFunc obterRespostas)
        {
            foreach (var questaoComplementar in questoesComplementares)
                yield return ObterQuestao(questaoComplementar.QuestaoComplementarId, dadosQuestionario, obterRespostas);
        }

        QuestaoDto ObterQuestao(long questaoId, IEnumerable<Questao> dadosQuestionario, ObterRespostasFunc obterRespostas)
        {
            var questao = dadosQuestionario.FirstOrDefault(c => c.Id == questaoId);

            return new QuestaoDto()
            {
                Id = questao.Id,
                Ordem = questao.Ordem,
                Nome = questao.Nome,
                TipoQuestao = questao.Tipo,
                Obrigatorio = questao.Obrigatorio,
                Observacao = questao.Observacao,
                Opcionais = questao.Opcionais,
                SomenteLeitura = questao.SomenteLeitura,
                OpcaoResposta = questao.OpcoesRespostas.Select(opcaoResposta =>
                {
                    return new OpcaoRespostaDto()
                    {
                        Id = opcaoResposta.Id,
                        Nome = opcaoResposta.Nome,
                        Ordem = opcaoResposta.Ordem,
                        QuestoesComplementares = opcaoResposta.QuestoesComplementares != null ?
                            ObterQuestoes(opcaoResposta.QuestoesComplementares, dadosQuestionario, obterRespostas)
                                .OrderBy(a => a.Ordem)
                                .ToList() :
                            null
                    };
                })
                .OrderBy(a => a.Ordem).ToArray(),
                Resposta = obterRespostas == null ? null :
                    obterRespostas(questaoId)
            };
        }
    }
}
