using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWorkflowCommand: IRequest<bool>
    {
        public ExcluirWorkflowCommand(long workflowId)
        {
            WorkflowId = workflowId;
        }

        public long WorkflowId { get; set; }
    }

    public class ExcluirWorkflowCommandValidator: AbstractValidator<ExcluirWorkflowCommand>
    {
        public ExcluirWorkflowCommandValidator()
        {
            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("É necessário informar o Id do workflow a excluir");
        }
    }
}
