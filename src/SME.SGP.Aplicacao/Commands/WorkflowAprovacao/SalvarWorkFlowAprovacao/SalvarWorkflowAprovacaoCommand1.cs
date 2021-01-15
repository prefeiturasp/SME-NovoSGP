using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarWorkflowAprovacaoCommand : IRequest<long>
    {
        public SalvarWorkflowAprovacaoCommand(WorkflowAprovacaoDto workflowAprovacao)
        {
            WorkflowAprovacao = workflowAprovacao;
        }

        public WorkflowAprovacaoDto WorkflowAprovacao { get; set; }
    }

    public  class SalvarWorkflowAprovacaoCommandValidator : AbstractValidator<SalvarWorkflowAprovacaoCommand>
    {
        public SalvarWorkflowAprovacaoCommandValidator()
        {
            RuleFor(c => c.WorkflowAprovacao)
               .NotEmpty()
               .WithMessage("O workflow deve ser informado para registro na base de dados.");

        }
    }
}
