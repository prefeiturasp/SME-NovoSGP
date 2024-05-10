using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasConselhoEmAprovacaoPorWorkflowQuery : IRequest<IEnumerable<WFAprovacaoNotaConselho>>
    {
        public ObterNotasConselhoEmAprovacaoPorWorkflowQuery(long workflowId)
        {
            WorkflowId = workflowId;
        }

        public long WorkflowId { get; }
    }

    public class ObterNotaConselhoEmAprovacaoPorWorkflowQueryValidator : AbstractValidator<ObterNotasConselhoEmAprovacaoPorWorkflowQuery>
    {
        public ObterNotaConselhoEmAprovacaoPorWorkflowQueryValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O id do Workflow de aprovação deve ser informado para consulta das notas em aprovação.");
        }
    }
}
