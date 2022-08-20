using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarLeituraNotificacaoCommand : IRequest
    {
        public NotificarLeituraNotificacaoCommand(Notificacao notificacao)
        {
            Notificacao = notificacao;
        }

        public Notificacao Notificacao { get; }
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
