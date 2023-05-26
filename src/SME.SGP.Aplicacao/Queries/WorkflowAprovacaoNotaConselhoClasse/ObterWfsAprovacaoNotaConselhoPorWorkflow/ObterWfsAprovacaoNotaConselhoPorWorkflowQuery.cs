using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterWfsAprovacaoNotaConselhoPorWorkflowQuery : IRequest<IEnumerable<WFAprovacaoNotaConselho>>
    {
        public ObterWfsAprovacaoNotaConselhoPorWorkflowQuery(long workflowId)
        {
            WorkflowId = workflowId;
        }

        public long WorkflowId { get; }
    }

    public class ObterNotasConselhoComAprovacaoPorWorkflowQueryValidator : AbstractValidator<ObterWfsAprovacaoNotaConselhoPorWorkflowQuery>
    {
        public ObterNotasConselhoComAprovacaoPorWorkflowQueryValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O id do Workflow de aprovação deve ser informado para consulta das notas em aprovação.");
        }
    }
}
