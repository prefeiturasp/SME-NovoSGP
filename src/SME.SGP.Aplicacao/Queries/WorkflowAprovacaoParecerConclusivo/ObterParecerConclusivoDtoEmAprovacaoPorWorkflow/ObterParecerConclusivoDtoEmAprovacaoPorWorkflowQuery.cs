using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQuery : IRequest<WFAprovacaoParecerConclusivoDto>
    {
        public ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQuery(long workflowId)
        {
            WorkflowId = workflowId;
        }

        public long WorkflowId { get; }
    }

    public class ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQueryValidator : AbstractValidator<ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQuery>
    {
        public ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQueryValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O identificador do workflow deve ser informado para consulta do parecer conclusivo em aprovação");
        }
    }
}
