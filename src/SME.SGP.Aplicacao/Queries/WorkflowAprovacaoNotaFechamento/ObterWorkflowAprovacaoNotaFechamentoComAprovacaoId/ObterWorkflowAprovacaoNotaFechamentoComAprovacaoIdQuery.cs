using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQuery : IRequest<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>>
    {
        public ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQuery(long workflowId)
        {
            WorkflowId = workflowId;
        }

        public long WorkflowId { get; }
    }

    public class ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQueryValidator : AbstractValidator<ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQuery>
    {
        public ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQueryValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O identificador do workflow deve ser informado para consulta de alteração de nota fechamento");
        }
    }
}
