using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotificacaoCommandValidator : AbstractValidator<SalvarNotificacaoCommand>
    {
        public SalvarNotificacaoCommandValidator()
        {
            RuleFor(c => c.Notificacao)
                .NotNull()
                .WithMessage("Os dados da notificação devem ser informados para serem salvos.");
        }
    }
}