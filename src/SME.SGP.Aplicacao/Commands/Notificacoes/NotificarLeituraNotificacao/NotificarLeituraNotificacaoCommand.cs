using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class NotificarLeituraNotificacaoCommand : IRequest
    {
        public NotificarLeituraNotificacaoCommand(Notificacao notificacao, string usuarioRf = null)
        {
            Notificacao = notificacao;
            UsuarioRf = usuarioRf;
        }

        public Notificacao Notificacao { get; }
        public string UsuarioRf { get; set; }
    }

    public class NotificarLeituraNotificacaoCommandValidator : AbstractValidator<NotificarLeituraNotificacaoCommand>
    {
        public NotificarLeituraNotificacaoCommandValidator()
        {
            RuleFor(x => x.Notificacao)
                .NotEmpty()
                .WithMessage("Deve ser informado a notificação para notificar a leitura");

            RuleFor(x => x.Notificacao.Codigo)
                .NotEmpty()
                .WithMessage("Deve ser informado o código da notificação para notificar a leitura");

            RuleFor(x => x.Notificacao.UsuarioId)
                .NotEmpty()
                .WithMessage("Deve ser informado o usurário da notificação para notificar a leitura");
        }
    }
}
