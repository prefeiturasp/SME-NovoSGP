using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterQuestoesObrigatoriasNaoPreechidasQueryHandler : IRequestHandler<ObterQuestoesObrigatoriasNaoPreechidasQuery, IEnumerable<QuestaoObrigatoriaDto>>
    {
        private List<QuestaoObrigatoriaDto> questoesObrigatorias;
        private readonly IMediator mediator;

        public ObterQuestoesObrigatoriasNaoPreechidasQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<QuestaoObrigatoriaDto>> Handle(ObterQuestoesObrigatoriasNaoPreechidasQuery request, CancellationToken cancellationToken)
        {
            questoesObrigatorias = new List<QuestaoObrigatoriaDto>();

            var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.Secao.QuestionarioId,
                                                questaoId =>
                                                    request.Questoes.Where(c => c.QuestaoId == questaoId)
                                                        .Select(respostaQuestao => new RespostaQuestaoDto
                                                        {
                                                            Id = GetHashCode(),
                                                            OpcaoRespostaId = 0,
                                                            Texto = respostaQuestao.Resposta,
                                                            Arquivo = null
                                                        })));

            ValidaRecursivo(request.Secao, string.Empty, questoes);

            return await Task.FromResult(questoesObrigatorias);
        }

        private void ValidaRecursivo(SecaoQuestionarioDto secao, string questaoPaiOrdem, IEnumerable<QuestaoDto> questoes)
        {
            foreach (var questao in questoes)
            {
                var ordem = questaoPaiOrdem != "" ? $"{questaoPaiOrdem}.{questao.Ordem.ToString()}" : questao.Ordem.ToString();

                if (EhQuestaoObrigatoriaNaoRespondida(questao))
                {
                    questoesObrigatorias.Add(new QuestaoObrigatoriaDto { SecaoId = secao.Id, NomeSecao = secao.Nome, Ordem = ordem });
                }
                else
                if (questao.OpcaoResposta.NaoNuloEContemRegistros() && questao.Resposta.NaoNuloEContemRegistrosRespondidos())
                {
                    foreach (var resposta in questao.Resposta)
                    {
                        var opcao = questao.OpcaoResposta.FirstOrDefault(opcao => opcao.Id == Convert.ToInt64(resposta.Texto));

                        if (opcao != null && opcao.QuestoesComplementares.Any())
                        {
                            ValidaRecursivo(secao, ordem, opcao.QuestoesComplementares);
                        }
                    }
                }
            }
        }

        private bool EhQuestaoObrigatoriaNaoRespondida(QuestaoDto questao)
        {
            return questao.Obrigatorio && !questao.Resposta.NaoNuloEContemRegistrosRespondidos();
        }
    }
}
