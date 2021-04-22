using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class AprovarItineranciaCommandValidator : AbstractValidator<AprovarItineranciaCommand>
    {
        public AprovarItineranciaCommandValidator()
        {
            RuleFor(c => c.ItineranciaId)
               .NotEmpty()
               .WithMessage("O ID da Itinerância deve ser informado");

            RuleFor(c => c.WorkflowId)
               .NotEmpty()
               .WithMessage("O ID do Workflow deve ser informado");
        }
    }
}
