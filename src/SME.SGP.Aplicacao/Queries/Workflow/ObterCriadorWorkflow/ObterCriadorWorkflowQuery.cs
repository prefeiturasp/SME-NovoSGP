using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCriadorWorkflowQuery : IRequest<string>
    {
        public ObterCriadorWorkflowQuery(long workflowId)
        {
            WorkflowId = workflowId;
        }

        public long WorkflowId { get; }
    }

    public class ObterCriadorWorkflowQueryValidator : AbstractValidator<ObterCriadorWorkflowQuery>
    {
        public ObterCriadorWorkflowQueryValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O identificador do workflow deve ser informado para consulta do seu criador");
        }
    }
}
