using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class EnviarFilaNotificacaoCriacaoPlanoAEECommandValidator : AbstractValidator<EnviarFilaNotificacaoCriacaoPlanoAEECommand>
    {
        public EnviarFilaNotificacaoCriacaoPlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE é necessário para envio da notificação de criação do plano AEE");
        }
    }
}
