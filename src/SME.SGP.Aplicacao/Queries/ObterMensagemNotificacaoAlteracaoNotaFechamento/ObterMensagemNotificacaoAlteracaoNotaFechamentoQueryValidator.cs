using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class
        ObterMensagemNotificacaoAlteracaoNotaFechamentoQueryValidator : AbstractValidator<
            ObterMensagemNotificacaoAlteracaoNotaFechamentoQuery>
    {
        public ObterMensagemNotificacaoAlteracaoNotaFechamentoQueryValidator()
        {
            RuleFor(c => c.WorkflowAprovacaoId)
                .GreaterThan(0)
                .WithMessage(
                    "O Id do workflow de aprovação deve ser informado para obter a mensagem da notificação de alteração de nota de fechamento.");

            RuleFor(c => c.NotificacaoId)
                .GreaterThan(0)
                .WithMessage(
                    "O Id da notificação deve ser informado para obter a mensagem da notificação de alteração de nota fechamento.");
        }
    }
}