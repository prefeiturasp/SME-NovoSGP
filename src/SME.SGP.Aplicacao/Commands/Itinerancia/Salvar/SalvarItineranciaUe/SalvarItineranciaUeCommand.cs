using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaUeCommand : IRequest<AuditoriaDto>
    {
        public SalvarItineranciaUeCommand(long ueId, long itineranciaId)
        {
            UeId = ueId;
            ItineranciaId = itineranciaId;
        }

        public long UeId { get; set; }
        public long ItineranciaId { get; set; }
    }
    public class SalvarItineranciaUeCommandValidator : AbstractValidator<SalvarItineranciaUeCommand>
    {
        public SalvarItineranciaUeCommandValidator()
        {
            RuleFor(x => x.UeId)
                   .GreaterThan(0)
                   .WithMessage("O id da UE deve ser informado!");
            RuleFor(x => x.ItineranciaId)
                   .GreaterThan(0)
                   .WithMessage("O id da itinerância deve ser informado!");
        }
    }
}
