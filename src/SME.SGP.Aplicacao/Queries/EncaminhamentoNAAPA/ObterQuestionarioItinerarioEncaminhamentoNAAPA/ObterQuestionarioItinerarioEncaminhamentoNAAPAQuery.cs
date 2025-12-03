using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioItinerarioEncaminhamentoNAAPAQuery : IRequest<AtendimentoNAAPASecaoItineranciaQuestoesDto>
    {
        public ObterQuestionarioItinerarioEncaminhamentoNAAPAQuery(long questionarioId, long? encaminhamentoSecaoId)
        {
            QuestionarioId = questionarioId;
            EncaminhamentoSecaoId = encaminhamentoSecaoId;
        }

        public long QuestionarioId { get; }
        public long? EncaminhamentoSecaoId { get; }
    }

    public class ObterQuestionarioItinerarioEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterQuestionarioItinerarioEncaminhamentoNAAPAQuery>
    {
        public ObterQuestionarioItinerarioEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.QuestionarioId)
            .NotEmpty()
            .WithMessage("O ID do questionário deve ser informado para consulta do questionário de itinerário do encaminhamento NAAPA.");
        }
    }
}
