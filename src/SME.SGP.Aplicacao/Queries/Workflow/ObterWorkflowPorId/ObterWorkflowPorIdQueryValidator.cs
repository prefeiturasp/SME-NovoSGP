using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowPorIdQueryValidator : AbstractValidator<ObterWorkflowPorIdQuery>
    {
        public ObterWorkflowPorIdQueryValidator()
        {
            RuleFor(c => c.WorkflowId)
            .NotEmpty()
            .WithMessage("O id do Workflow deve ser informado.");

        }
    }
}
