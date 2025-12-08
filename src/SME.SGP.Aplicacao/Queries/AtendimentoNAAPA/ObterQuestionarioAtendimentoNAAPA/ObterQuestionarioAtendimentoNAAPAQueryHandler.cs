using MediatR;
using SME.SGP.Dominio;
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
    public class ObterQuestionarioAtendimentoNAAPAQueryHandler : IRequestHandler<ObterQuestionarioAtendimentoNAAPAQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamento;

        public ObterQuestionarioAtendimentoNAAPAQueryHandler(IMediator mediator, IRepositorioQuestaoAtendimentoNAAPA repositorioQuestaoEncaminhamento, IRepositorioQuestionario repositorioQuestionario)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoEncaminhamento = repositorioQuestaoEncaminhamento ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamento));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestionarioAtendimentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            var respostasEncaminhamento = request.EncaminhamentoId.HasValue ?
                await repositorioQuestaoEncaminhamento.ObterRespostasEncaminhamento(request.EncaminhamentoId.Value) :
                Enumerable.Empty<RespostaQuestaoAtendimentoNAAPADto>();

            var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.QuestionarioId , questaoId =>
                respostasEncaminhamento.Where(c => c.QuestaoId == questaoId)
                .Select(respostaEncaminhamento =>
                {
                    return new RespostaQuestaoDto()
                    {
                        Id = respostaEncaminhamento.Id,
                        OpcaoRespostaId = respostaEncaminhamento.RespostaId,
                        Texto = respostaEncaminhamento.Texto,
                        Arquivo = respostaEncaminhamento.Arquivo
                    };
                })));

            questoes = await AplicarRegrasEncaminhamento(request.QuestionarioId, questoes, request.CodigoAluno, request.CodigoTurma);

            return questoes;
        }

        private async Task<IEnumerable<QuestaoDto>> AplicarRegrasEncaminhamento(long questionarioId, IEnumerable<QuestaoDto> questoes, string codigoAluno, string codigoTurma)
        {
            if (questionarioId == 1)
            {
                if (await ValidarFrequenciaGlobalAlunoInsuficiente(codigoAluno, codigoTurma))
                {
                    var questaoJustificativa = ObterQuestaoJustificativa(questoes);
                    questaoJustificativa.Obrigatorio = true;
                } else
                    return RemoverQuestaoJustificativa(questoes);
            }

            return questoes;
        }

        private IEnumerable<QuestaoDto> RemoverQuestaoJustificativa(IEnumerable<QuestaoDto> questoes)
        {
            var questaoJustificativa = ObterQuestaoJustificativa(questoes);
            return questoes.Where(c => c.Id != questaoJustificativa.Id);
        }

        private QuestaoDto ObterQuestaoJustificativa(IEnumerable<QuestaoDto> questoes)
            => questoes.FirstOrDefault(c => c.Id == 2);

        private async Task<bool> ValidarFrequenciaGlobalAlunoInsuficiente(string codigoAluno, string codigoTurma)
        {
            var frequenciaGlobal = await mediator.Send(new ObterFrequenciaGeralAlunoQuery(codigoAluno, codigoTurma));

            var tipoParametroFrequenciaMinima = await ObterTipoParametroFrequenciaMinima(codigoTurma);

            var parametroPercentualFrequenciaCritico = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(tipoParametroFrequenciaMinima, DateTime.Now.Year));
            var percentualFrequenciaCritico = double.Parse(parametroPercentualFrequenciaCritico.Valor);

            return frequenciaGlobal < percentualFrequenciaCritico;
        }

        private async Task<TipoParametroSistema> ObterTipoParametroFrequenciaMinima(string codigoTurma)
        {
            return await mediator.Send(new ObterModalidadeTurmaPorCodigoQuery(codigoTurma)) == Modalidade.EducacaoInfantil ? 
                TipoParametroSistema.PercentualFrequenciaMinimaInfantil : 
                TipoParametroSistema.PercentualFrequenciaCritico;
        }

        QuestaoDto ObterQuestao(long questaoId, IEnumerable<Questao> dadosQuestionario, IEnumerable<RespostaQuestaoAtendimentoNAAPADto> respostasEncaminhamento)
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
                        QuestoesComplementares = opcaoResposta.QuestoesComplementares.NaoEhNulo() ?
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

        private IEnumerable<QuestaoDto> ObterQuestoes(List<OpcaoQuestaoComplementar> questoesComplementares, IEnumerable<Questao> dadosQuestionario, IEnumerable<RespostaQuestaoAtendimentoNAAPADto> respostasEncaminhamento)
        {
            foreach (var questaoComplementar in questoesComplementares)
                yield return ObterQuestao(questaoComplementar.QuestaoComplementarId, dadosQuestionario, respostasEncaminhamento);
        }
    }
}
