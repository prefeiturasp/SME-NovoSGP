using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SalvarWorkflowAprovacaoItineranciaCommandValidator : AbstractValidator<SalvarWorkflowAprovacaoItineranciaCommand>
    {
        public SalvarWorkflowAprovacaoItineranciaCommandValidator()
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
