using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacaoPorIdQueryValidator : AbstractValidator<ObterNotificacaoPorIdQuery>
    {
        public ObterNotificacaoPorIdQueryValidator()
        {
            RuleFor(c => c.NotificacaoId)
                .GreaterThan(0)
                .WithMessage("O Id da notificação deve ser informado para obter a notificação");
        }
    }
}