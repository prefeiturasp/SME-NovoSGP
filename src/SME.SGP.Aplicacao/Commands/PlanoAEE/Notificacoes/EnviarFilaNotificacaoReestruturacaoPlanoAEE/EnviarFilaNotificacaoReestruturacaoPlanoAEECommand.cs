using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class EnviarFilaNotificacaoReestruturacaoPlanoAEECommand : IRequest<bool>
    {
        public EnviarFilaNotificacaoReestruturacaoPlanoAEECommand(long reestruturacaoId)
        {
            ReestruturacaoId = reestruturacaoId;
        }

        public long ReestruturacaoId { get; }
    }

    public class EnviarFilaNotificacaoReestruturacaoPlanoAEECommandValidator : AbstractValidator<EnviarFilaNotificacaoReestruturacaoPlanoAEECommand>
    {
        public EnviarFilaNotificacaoReestruturacaoPlanoAEECommandValidator()
        {
            RuleFor(a => a.ReestruturacaoId)
                .NotEmpty()
                .WithMessage("O id da reestruturação é necessário para envio da notificação de reestruturação do plano AEE");
        }
    }
}
