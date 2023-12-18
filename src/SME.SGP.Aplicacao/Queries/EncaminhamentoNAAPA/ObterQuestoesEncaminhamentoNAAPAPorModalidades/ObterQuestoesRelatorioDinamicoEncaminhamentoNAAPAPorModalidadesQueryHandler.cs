using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Infra.Dtos.Questionario;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQueryHandler : IRequestHandler<ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery, IEnumerable<SecaoQuestoesDTO>>
    {
        private readonly IMediator mediator;

        public ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<SecaoQuestoesDTO>> Handle(ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery request, CancellationToken cancellationToken)
        {
            var secoesQuestionario = await mediator.Send(new ObterSecoesEncaminhamentoNAAPAPorModalidadesQuery(TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA, 
                                                                                                               request.ModalidadesId));

            var secoesQuestoesRetorno = new List<SecaoQuestoesDTO>();
            foreach (var secaoQuestionario in secoesQuestionario)
            {
                var questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secaoQuestionario.QuestionarioId));
                secoesQuestoesRetorno.Add(new SecaoQuestoesDTO()
                {
                    Id = secaoQuestionario.Id,
                    Nome = secaoQuestionario.Nome,
                    NomeComponente = secaoQuestionario.NomeComponente,
                    Ordem = secaoQuestionario.Ordem,
                    QuestionarioId = secaoQuestionario.QuestionarioId,
                    TipoQuestionario = secaoQuestionario.TipoQuestionario,
                    Questoes = questoes,
                    ModalidadesCodigo = secaoQuestionario.ModalidadesCodigo
                });
            }
                

            return secoesQuestoesRetorno;
        }
    }
}
