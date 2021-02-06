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
    public class ObterQuestionarioEncaminhamentoAeeQueryHandler : IRequestHandler<ObterQuestionarioEncaminhamentoAeeQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamento;

        public ObterQuestionarioEncaminhamentoAeeQueryHandler(IMediator mediator, IRepositorioQuestaoEncaminhamentoAEE repositorioQuestaoEncaminhamento)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoEncaminhamento = repositorioQuestaoEncaminhamento ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamento));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestionarioEncaminhamentoAeeQuery request, CancellationToken cancellationToken)
        {
            var dadosQuestionario = await repositorioQuestaoEncaminhamento.ObterListaPorQuestionario(request.QuestionarioId);

            var questoesComplementares = dadosQuestionario
                .Where(dq => dq.OpcoesRespostas.Any(a => a.QuestoesComplementares.Any()))
                .SelectMany(dq => dq.OpcoesRespostas.Where(c => c.QuestoesComplementares.Any()).SelectMany(a => a.QuestoesComplementares.Select(q => q.QuestaoComplementarId)))
                .Distinct();

            var respostasEncaminhamento = request.EncaminhamentoId.HasValue ?
                await repositorioQuestaoEncaminhamento.ObterRespostasEncaminhamento(request.EncaminhamentoId.Value) :
                Enumerable.Empty<RespostaQuestaoEncaminhamentoAEEDto>();

            var questoes = dadosQuestionario
                .Where(dq => !questoesComplementares.Contains(dq.Id))
                .Select(dq => ObterQuestao(dq.Id, dadosQuestionario, respostasEncaminhamento))
                .OrderBy(q => q.Ordem)
                .ToArray();

            await AplicarRegrasEncaminhamento(request.QuestionarioId, questoes, request.CodigoAluno, request.CodigoTurma);

            return questoes;
        }

        private async Task AplicarRegrasEncaminhamento(long questionarioId, QuestaoDto[] questoes, string codigoAluno, string codigoTurma)
        {
            if (questionarioId == 1 && await ValidarFrequenciaGlobalAlunoInsuficiente(codigoAluno, codigoTurma))
            {
                var questaoJustificativa = ObterQuestaoJustificativa(questoes);
                questaoJustificativa.Obrigatorio = true;
            }
        }

        private QuestaoDto ObterQuestaoJustificativa(QuestaoDto[] questoes)
            => questoes.FirstOrDefault(c => c.Id == 2);

        private async Task<bool> ValidarFrequenciaGlobalAlunoInsuficiente(string codigoAluno, string codigoTurma)
        {
            var frequenciaGlobal = await mediator.Send(new ObterFrequenciaGeralAlunoQuery(codigoAluno, codigoTurma));
            var parametroPercentualFrequenciaCritico = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PercentualFrequenciaCritico, DateTime.Now.Year));
            var percentualFrequenciaCritico = double.Parse(parametroPercentualFrequenciaCritico.Valor);

            return frequenciaGlobal < percentualFrequenciaCritico;
        }

        QuestaoDto ObterQuestao(long questaoId, IEnumerable<Questao> dadosQuestionario, IEnumerable<RespostaQuestaoEncaminhamentoAEEDto> respostasEncaminhamento)
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
                OpcaoResposta = questao.OpcoesRespostas.Select(opcaoResposta =>
                {
                    return new OpcaoRespostaDto()
                    {
                        Id = opcaoResposta.Id,
                        Nome = opcaoResposta.Nome,
                        Ordem = opcaoResposta.Ordem,
                        Observacao = opcaoResposta.Observacao,
                        QuestoesComplementares = opcaoResposta.QuestoesComplementares != null ?
                            ObterQuestoes(opcaoResposta.QuestoesComplementares, dadosQuestionario, respostasEncaminhamento).ToList() :
                            null
                    };
                })
                .OrderBy(a => a.Ordem).ToArray(),
                Resposta = respostasEncaminhamento.Where(c => c.QuestaoId == questaoId).Select(respostaEncaminhamento =>
                {
                    return new RespostaQuestaoDto()
                    {
                        Id = respostaEncaminhamento.Id,
                        OpcaoRespostaId = respostaEncaminhamento.RespostaId,
                        Texto = respostaEncaminhamento.Texto,
                        Arquivo = respostaEncaminhamento.Arquivo
                    };
                }).ToArray()
            };

        }

        private IEnumerable<QuestaoDto> ObterQuestoes(List<OpcaoQuestaoComplementar> questoesComplementares, IEnumerable<Questao> dadosQuestionario, IEnumerable<RespostaQuestaoEncaminhamentoAEEDto> respostasEncaminhamento)
        {
            foreach (var questaoComplementar in questoesComplementares)
                yield return ObterQuestao(questaoComplementar.QuestaoComplementarId, dadosQuestionario, respostasEncaminhamento);
        }
    }
}
