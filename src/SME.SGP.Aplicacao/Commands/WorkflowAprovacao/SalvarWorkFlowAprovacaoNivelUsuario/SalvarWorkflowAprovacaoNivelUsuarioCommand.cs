using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarWorkflowAprovacaoNivelUsuarioCommand : IRequest<long>
    {
        public SalvarWorkflowAprovacaoNivelUsuarioCommand(WorkflowAprovacaoDto workflowAprovacao)
        {
            WorkflowAprovacao = workflowAprovacao;
        }

        public WorkflowAprovacaoDto WorkflowAprovacao { get; set; }
    }

    public  class SalvarWorkflowAprovacaoNivelUsuarioCommandValidator : AbstractValidator<SalvarWorkflowAprovacaoNivelUsuarioCommand>
    {
        public SalvarWorkflowAprovacaoNivelUsuarioCommandValidator()
        {
            RuleFor(c => c.WorkflowAprovacao)
               .NotEmpty()
               .WithMessage("O workflow deve ser informado para registro na base de dados.");
        }
    }
}
