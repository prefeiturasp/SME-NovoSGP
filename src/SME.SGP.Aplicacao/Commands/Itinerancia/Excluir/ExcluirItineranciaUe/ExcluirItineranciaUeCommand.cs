using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaUeCommand : IRequest<bool>
    {
        public ExcluirItineranciaUeCommand(long ueId, long itineranciaId)
        {
            UeId = ueId;
            ItineranciaId = itineranciaId;
        }

        public long UeId { get; set; }
        public long ItineranciaId { get; set; }

    }
    public class ExcluirItineranciaUeCommandValidator : AbstractValidator<ExcluirItineranciaUeCommand>
    {
        public ExcluirItineranciaUeCommandValidator()
        {
            RuleFor(c => c.UeId)
            .GreaterThan(0)
            .WithMessage("O id da UE da itinerância deve ser informado para exclusão.");

            RuleFor(c => c.ItineranciaId)
                .GreaterThan(0)
                .WithMessage("O id da itinerância deve ser informado para exclusão.");
        }
    }
}
