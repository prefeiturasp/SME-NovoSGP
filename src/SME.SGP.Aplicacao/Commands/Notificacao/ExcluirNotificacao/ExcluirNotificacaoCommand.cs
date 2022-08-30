using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoCommand : IRequest
    {
        public ExcluirNotificacaoCommand(Notificacao notificacao)
        {
            Notificacao = notificacao;
        }

        public Notificacao Notificacao { get; }
    }

    public class ExcluirNotificacaoCommandValidator : AbstractValidator<ExcluirNotificacaoCommand>
    {
        public ExcluirNotificacaoCommandValidator()
        {
            RuleFor(x => x.Notificacao)
                .NotEmpty()
                .WithMessage("A notificação deve ser informada para exclusão");
        }
    }
}
