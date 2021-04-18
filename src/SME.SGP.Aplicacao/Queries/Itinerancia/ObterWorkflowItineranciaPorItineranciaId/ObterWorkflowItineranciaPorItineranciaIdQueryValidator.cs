using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
     public class ObterWorkflowItineranciaPorItineranciaIdQueryValidator : AbstractValidator<ObterWorkflowItineranciaPorItineranciaIdQuery>
    {
        public ObterWorkflowItineranciaPorItineranciaIdQueryValidator()
        {
            RuleFor(c => c.ItineranciaId)
            .NotEmpty()
            .WithMessage("O id da itinerância deve ser informado.");

        }
    }
}
