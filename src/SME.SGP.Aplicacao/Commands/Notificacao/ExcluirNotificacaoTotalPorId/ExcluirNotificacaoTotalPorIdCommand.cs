using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoTotalPorIdCommand : IRequest<bool>
    {
        public ExcluirNotificacaoTotalPorIdCommand(long notificacaoId)
        {
            NotificacaoId = notificacaoId;
        }

        public long NotificacaoId { get; set; }
    }

    public class ExcluirNotificacaoTotalPorIdCommandValidator : AbstractValidator<ExcluirNotificacaoTotalPorIdCommand>
    {
        public ExcluirNotificacaoTotalPorIdCommandValidator()
        {
            RuleFor(a => a.NotificacaoId)
                .NotEmpty()
                .WithMessage("O id da notificação deve ser informado para exclusão");
        }
    }
}
