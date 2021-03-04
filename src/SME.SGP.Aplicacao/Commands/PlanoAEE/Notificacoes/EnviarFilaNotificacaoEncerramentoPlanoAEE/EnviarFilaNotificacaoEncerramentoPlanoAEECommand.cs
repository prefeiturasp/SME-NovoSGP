using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class EnviarFilaNotificacaoEncerramentoPlanoAEECommand : IRequest<bool>
    {
        public EnviarFilaNotificacaoEncerramentoPlanoAEECommand(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }

        public long PlanoAEEId { get; }
    }

    public class EnviarFilaNotificacaoEncerramentoPlanoAEECommandValidator : AbstractValidator<EnviarFilaNotificacaoEncerramentoPlanoAEECommand>
    {
        public EnviarFilaNotificacaoEncerramentoPlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para notificar seu encerramento.");
        }
    }
}
