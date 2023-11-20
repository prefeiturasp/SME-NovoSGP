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
    public class ObterQuestionarioRegistroAcaoQueryHandler : IRequestHandler<ObterQuestionarioRegistroAcaoQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioQuestaoRegistroAcaoBuscaAtiva repositorioQuestaoRegistroAcao;

        public ObterQuestionarioRegistroAcaoQueryHandler(IMediator mediator, IRepositorioQuestaoRegistroAcaoBuscaAtiva repositorioQuestaoRegistroAcao, IRepositorioQuestionario repositorioQuestionario)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoRegistroAcao = repositorioQuestaoRegistroAcao ?? throw new ArgumentNullException(nameof(repositorioQuestaoRegistroAcao));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestionarioRegistroAcaoQuery request, CancellationToken cancellationToken)
        {
            var respostasRegistroAcao = request.RegistroAcaoId.HasValue ?
                await repositorioQuestaoRegistroAcao.ObterRespostasRegistroAcao(request.RegistroAcaoId.Value) :
                Enumerable.Empty<RespostaQuestaoRegistroAcaoBuscaAtivaDto>();

            var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.QuestionarioId , questaoId =>
                respostasRegistroAcao.Where(c => c.QuestaoId == questaoId)
                .Select(respostaRegistroAcao =>
                {
                    return new RespostaQuestaoDto()
                    {
                        Id = respostaRegistroAcao.Id,
                        OpcaoRespostaId = respostaRegistroAcao.RespostaId,
                        Texto = respostaRegistroAcao.Texto,
                        Arquivo = respostaRegistroAcao.Arquivo
                    };
                })));

            return questoes;
        }

        QuestaoDto ObterQuestao(long questaoId, IEnumerable<Questao> dadosQuestionario, IEnumerable<RespostaQuestaoEncaminhamentoNAAPADto> respostasEncaminhamento)
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

        private IEnumerable<QuestaoDto> ObterQuestoes(List<OpcaoQuestaoComplementar> questoesComplementares, IEnumerable<Questao> dadosQuestionario, IEnumerable<RespostaQuestaoEncaminhamentoNAAPADto> respostasEncaminhamento)
        {
            foreach (var questaoComplementar in questoesComplementares)
                yield return ObterQuestao(questaoComplementar.QuestaoComplementarId, dadosQuestionario, respostasEncaminhamento);
        }
    }
}
