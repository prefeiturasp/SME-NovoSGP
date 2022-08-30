using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class NotificarCriacaoNotificacaoCommand : IRequest
    {
        public NotificarCriacaoNotificacaoCommand(Notificacao notificacao, string usuarioRf = "")
        {
            Notificacao = notificacao;
            UsuarioRf = usuarioRf;
        }

        public Notificacao Notificacao { get; }
        public string UsuarioRf { get; }
    }

    public class NotificarCriacaoNotificacaoCommandValidator : AbstractValidator<NotificarCriacaoNotificacaoCommand>
    {
        public NotificarCriacaoNotificacaoCommandValidator()
        {
            RuleFor(x => x.Notificacao)
                .NotEmpty()
                .WithMessage("A notificação gerada deve ser informada para notificar sua geração");

            RuleFor(x => x.Notificacao.Codigo)
                .NotEmpty()
                .WithMessage("O código da notificação deve ser informado para notificar sua geração");

            RuleFor(x => x.Notificacao.Titulo)
                .NotEmpty()
                .WithMessage("O título da notificação deve ser informado para notificar sua geração");

            RuleFor(x => x.Notificacao.CriadoEm)
                .NotEmpty()
                .WithMessage("A data de criação da notificação deve ser informada para notificar sua geração");

            RuleFor(x => x.UsuarioRf)
                .NotEmpty()
                .When(x => !x.Notificacao.UsuarioId.HasValue)
                .WithMessage("O RF do usuário deve ser informado para notificação");
        }
    }
}
