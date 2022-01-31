using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaConselhoEmAprovacaoPorWorkflowQuery : IRequest<WFAprovacaoNotaConselho>
    {
        public ObterNotaConselhoEmAprovacaoPorWorkflowQuery(long workflowId)
        {
            WorkflowId = workflowId;
        }

        public long WorkflowId { get; }
    }

    public class ObterNotaConselhoEmAprovacaoPorWorkflowQueryValidator : AbstractValidator<ObterNotaConselhoEmAprovacaoPorWorkflowQuery>
    {
        public ObterNotaConselhoEmAprovacaoPorWorkflowQueryValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O id do Workflow de aprovação deve ser informado para consulta das notas em aprovação.");
        }
    }
}
