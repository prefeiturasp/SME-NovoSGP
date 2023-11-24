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
 
    public class ObterQuestoesObrigatoriasNaoRespondidasQueryHandler : IRequestHandler<ObterQuestoesObrigatoriasNaoRespondidasQuery, IEnumerable<QuestaoObrigatoriaNaoRespondidaDto>>
    {
        private readonly IMediator mediator;

        public ObterQuestoesObrigatoriasNaoRespondidasQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<QuestaoObrigatoriaNaoRespondidaDto>> Handle(ObterQuestoesObrigatoriasNaoRespondidasQuery request, CancellationToken cancellationToken)
        {
            List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir = new List<QuestaoObrigatoriaNaoRespondidaDto>();
            var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.Secao.QuestionarioId,
                     questaoId =>
                         request.Respostas.Where(c => c.QuestaoId == questaoId)
                             .Select(respostaQuestao => new RespostaQuestaoDto
                             {
                                 Id = GetHashCode(),
                                 OpcaoRespostaId = 0,
                                 Texto = respostaQuestao.Resposta,
                                 Arquivo = null
                             })));

            if (request.AddQuestoesObrigatoriasNaoPreenchidas.NaoEhNulo())
                await request.AddQuestoesObrigatoriasNaoPreenchidas(request.Secao, questoes, questoesObrigatoriasAConsistir);

            ValidaRecursivo(request.Secao, "", questoes, questoesObrigatoriasAConsistir);

            return questoesObrigatoriasAConsistir;
        }

        private bool EhQuestaoObrigatoriaNaoRespondida(QuestaoDto questao)
        {
            return questao.Obrigatorio && !questao.Resposta.NaoNuloEContemRegistrosRespondidos();
        }

        private void ValidaRecursivo(SecaoQuestionarioDto secao, string questaoPaiOrdem, IEnumerable<QuestaoDto> questoes, List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir)
        {
            foreach (var questao in questoes)
            {
                var ordem = questaoPaiOrdem != "" ? $"{questaoPaiOrdem}.{questao.Ordem.ToString()}" : questao.Ordem.ToString();

                if (EhQuestaoObrigatoriaNaoRespondida(questao))
                {
                    questoesObrigatoriasAConsistir.Add(new QuestaoObrigatoriaNaoRespondidaDto(secao.Id, secao.Nome, ordem));
                }
                else if (questao.OpcaoResposta.NaoNuloEContemRegistros() && questao.Resposta.NaoNuloEContemRegistrosRespondidos())
                {
                    foreach (var resposta in questao.Resposta)
                    {
                        var opcao = questao.OpcaoResposta.FirstOrDefault(opcao => opcao.Id == Convert.ToInt64(resposta.Texto));

                        if (opcao.NaoEhNulo() && opcao.QuestoesComplementares.Any())
                        {
                            ValidaRecursivo(secao, ordem, opcao.QuestoesComplementares, questoesObrigatoriasAConsistir);
                        }
                    }
                }
            }
        }
    }
}
