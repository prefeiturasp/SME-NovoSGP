using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery : IRequest<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>>
    {
        public ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery(long workflowId)
        {
            WorkflowId = workflowId; 
        }
        public long WorkflowId { get; internal set; }
    }

    public class ObterNotaFechamentoEmAprovacaoPorWorkflowIdQueryValidator : AbstractValidator<ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery>
    {
        public ObterNotaFechamentoEmAprovacaoPorWorkflowIdQueryValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O workflowId deve ser informado."); 
        }
    }
}