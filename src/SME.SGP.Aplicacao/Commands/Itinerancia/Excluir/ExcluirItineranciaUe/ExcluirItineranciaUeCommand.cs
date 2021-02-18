using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaUeCommand : IRequest<bool>
    {
        public ExcluirItineranciaUeCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
    public class ExcluirItineranciaUeCommandValidator : AbstractValidator<ExcluirItineranciaUeCommand>
    {
        public ExcluirItineranciaUeCommandValidator()
        {
            RuleFor(c => c.Id)
            .GreaterThan(0)
            .WithMessage("O id da UE da itinerância deve ser informado para exclusão.");
        }
    }
}
