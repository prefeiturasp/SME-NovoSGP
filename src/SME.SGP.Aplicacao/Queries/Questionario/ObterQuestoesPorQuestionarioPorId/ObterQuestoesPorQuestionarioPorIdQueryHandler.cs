using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public delegate IEnumerable<RespostaQuestaoDto> ObterRespostasFunc(long questaoId);

    public class ObterQuestoesPorQuestionarioPorIdQueryHandler : IRequestHandler<ObterQuestoesPorQuestionarioPorIdQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IRepositorioQuestionario repositorioQuestionario;
        private readonly IRepositorioCache repositorioCache;

        public ObterQuestoesPorQuestionarioPorIdQueryHandler(IRepositorioQuestionario repositorioQuestionario, IRepositorioCache repositorioCache)
        {
            this.repositorioQuestionario = repositorioQuestionario ?? throw new ArgumentNullException(nameof(repositorioQuestionario));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestoesPorQuestionarioPorIdQuery request, CancellationToken cancellationToken)
        {
            var dadosQuestionario = await repositorioCache.ObterAsync(string.Format(NomeChaveCache.CHAVE_QUESTIONARIO, request.QuestionarioId),
                async () => await repositorioQuestionario.ObterQuestoesPorQuestionarioId(request.QuestionarioId),
                "Obter questionário");

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
                Dimensao = questao.Dimensao,
                Tamanho = questao.Tamanho,
                PlaceHolder = questao.PlaceHolder,
                Mascara = questao.Mascara,
                NomeComponente = questao.NomeComponente,
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
