using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoItineranciaPorIdQueryValidator : AbstractValidator<ObterWorkflowAprovacaoItineranciaPorIdQuery>
    {
        public ObterWorkflowAprovacaoItineranciaPorIdQueryValidator()
        {
            RuleFor(c => c.WorkflowId)
            .NotEmpty()
            .WithMessage("O id do Workflow deve ser informado.");

        }
    }
}
