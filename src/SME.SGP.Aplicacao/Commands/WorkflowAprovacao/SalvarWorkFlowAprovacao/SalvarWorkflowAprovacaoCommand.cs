using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarWorkflowAprovacaoCommand : IRequest<long>
    {
        public SalvarWorkflowAprovacaoCommand(WorkflowAprovacao workflowAprovacao)
        {
            WorkflowAprovacao = workflowAprovacao;
        }

        public WorkflowAprovacao WorkflowAprovacao { get; set; }
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
