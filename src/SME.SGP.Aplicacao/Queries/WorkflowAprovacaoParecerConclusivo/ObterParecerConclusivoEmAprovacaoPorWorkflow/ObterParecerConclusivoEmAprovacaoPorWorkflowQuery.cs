using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterParecerConclusivoEmAprovacaoPorWorkflowQuery : IRequest<WFAprovacaoParecerConclusivo>
    {
        public ObterParecerConclusivoEmAprovacaoPorWorkflowQuery(long workflowId)
        {
            WorkflowId = workflowId;
        }

        public long WorkflowId { get; }
    }

    public class ObterParecerConclusivoEmAprovacaoPorWorkflowQueryValidator : AbstractValidator<ObterParecerConclusivoEmAprovacaoPorWorkflowQuery>
    {
        public ObterParecerConclusivoEmAprovacaoPorWorkflowQueryValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O identificador do workflow deve ser informado para consulta do parecer conclusivo em aprovação");
        }
    }
}
