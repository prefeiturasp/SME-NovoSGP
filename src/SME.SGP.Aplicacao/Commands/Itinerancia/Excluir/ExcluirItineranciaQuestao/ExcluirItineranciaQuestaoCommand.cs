using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaQuestaoCommand : IRequest<bool>
    {
        public ExcluirItineranciaQuestaoCommand(long questaoId, long itineranciaId)
        {
            QuestaoId = questaoId;
            ItineranciaId = itineranciaId;
        }

        public long QuestaoId { get; set; }

        public long ItineranciaId { get; set; }
    }
    public class ExcluirItineranciaQuestaoCommandValidator : AbstractValidator<ExcluirItineranciaQuestaoCommand>
    {
        public ExcluirItineranciaQuestaoCommandValidator()
        {
            RuleFor(c => c.QuestaoId)
            .GreaterThan(0)
            .WithMessage("O id da questão da itinerância deve ser informado para exclusão.");

            RuleFor(c => c.ItineranciaId)
            .GreaterThan(0)
            .WithMessage("O id da itinerância deve ser informado para exclusão.");
        }
    }
}
