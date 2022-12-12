using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoPorNotificacaoIdQueryValidator : AbstractValidator<ObterWorkflowAprovacaoPorNotificacaoIdQuery>
    {
        public ObterWorkflowAprovacaoPorNotificacaoIdQueryValidator()
        {
            RuleFor(c => c.NotificacaoId)
                .GreaterThan(0)
                .WithMessage("O Id da notificação deve ser informado para obter o Workflow de aprovação.");
        }
    }
}