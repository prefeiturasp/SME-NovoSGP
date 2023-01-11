using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class
        ObterMensagemNotificacaoAlteracaoParecerConclusivoQueryValidator : AbstractValidator<
            ObterMensagemNotificacaoAlteracaoParecerConclusivoQuery>
    {
        public ObterMensagemNotificacaoAlteracaoParecerConclusivoQueryValidator()
        {
            RuleFor(c => c.WorkflowAprovacaoId)
                .GreaterThan(0)
                .WithMessage(
                    "O Id do workflow de aprovação deve ser informado para obter a mensagem da notificação de alteração do parecer conclusivo.");

            RuleFor(c => c.NotificacaoId)
                .GreaterThan(0)
                .WithMessage(
                    "O Id da notificação deve ser informado para obter a mensagem da notificação de alteração do parecer conclusivo.");
        }
    }
}