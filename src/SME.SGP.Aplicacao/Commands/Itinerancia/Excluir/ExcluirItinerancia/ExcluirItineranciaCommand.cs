using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaCommand : IRequest<bool>
    {
        public ExcluirItineranciaCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
    public class ExcluirItineranciaCommandValidator : AbstractValidator<ExcluirItineranciaCommand>
    {
        public ExcluirItineranciaCommandValidator()
        {
            RuleFor(c => c.Id)
            .GreaterThan(0)
            .WithMessage("O id da itinerância deve ser informado para exclusão.");
        }
    }
}
