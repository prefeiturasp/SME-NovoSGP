using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaQuestaoCommand : IRequest<bool>
    {
        public ExcluirItineranciaQuestaoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
    public class ExcluirItineranciaQuestaoCommandValidator : AbstractValidator<ExcluirItineranciaQuestaoCommand>
    {
        public ExcluirItineranciaQuestaoCommandValidator()
        {
            RuleFor(c => c.Id)
            .GreaterThan(0)
            .WithMessage("O id da questão da itinerância deve ser informado para exclusão.");
        }
    }
}
