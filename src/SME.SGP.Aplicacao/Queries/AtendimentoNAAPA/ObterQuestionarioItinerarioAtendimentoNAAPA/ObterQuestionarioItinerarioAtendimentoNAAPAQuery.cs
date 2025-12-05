using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioItinerarioAtendimentoNAAPAQuery : IRequest<AtendimentoNAAPASecaoItineranciaQuestoesDto>
    {
        public ObterQuestionarioItinerarioAtendimentoNAAPAQuery(long questionarioId, long? encaminhamentoSecaoId)
        {
            QuestionarioId = questionarioId;
            EncaminhamentoSecaoId = encaminhamentoSecaoId;
        }

        public long QuestionarioId { get; }
        public long? EncaminhamentoSecaoId { get; }
    }

    public class ObterQuestionarioItinerarioAtendimentoNAAPAQueryValidator : AbstractValidator<ObterQuestionarioItinerarioAtendimentoNAAPAQuery>
    {
        public ObterQuestionarioItinerarioAtendimentoNAAPAQueryValidator()
        {
            RuleFor(c => c.QuestionarioId)
            .NotEmpty()
            .WithMessage("O ID do questionário deve ser informado para consulta do questionário de itinerário do atendimento NAAPA.");
        }
    }
}
