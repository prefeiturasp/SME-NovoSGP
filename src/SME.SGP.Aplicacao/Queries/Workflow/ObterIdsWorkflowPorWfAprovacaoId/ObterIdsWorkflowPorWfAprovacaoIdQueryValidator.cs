using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsWorkflowPorWfAprovacaoIdQueryValidator : AbstractValidator<ObterIdsWorkflowPorWfAprovacaoIdQuery>
    {
        public ObterIdsWorkflowPorWfAprovacaoIdQueryValidator()
        {
            RuleFor(c => c.WorkflowId)
            .NotEmpty()
            .WithMessage("O id do Workflow deve ser informado.");
            RuleFor(c => c.TabelaVinculada)
            .NotEmpty()
            .WithMessage("O nome da tabela vinculada ao Workflow deve ser informada.");

        }
    }
}
