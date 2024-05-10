using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoPorIdCommand : IRequest<bool>
    {
        public ExcluirNotificacaoPorIdCommand(long Id)
        {
            this.Id = Id;
        }

        public long Id { get; }
    }

    public class ExcluirNotificacaoPorIdCommandValidator : AbstractValidator<ExcluirNotificacaoPorIdCommand>
    {
        public ExcluirNotificacaoPorIdCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id da notificação deve ser informado para exclusão");
        }
    }
}
