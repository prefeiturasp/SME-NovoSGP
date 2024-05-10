using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class NotificarEncerramentoPlanoAEECommand : IRequest<bool>
    {
        public NotificarEncerramentoPlanoAEECommand(long planoAEEId, Usuario usuario)
        {
            PlanoAEEId = planoAEEId;
            Usuario = usuario;
        }

        public long PlanoAEEId { get; }
        public Usuario Usuario { get; }
    }

    public class NotificarEncerramentoPlanoAEECommandValidator : AbstractValidator<NotificarEncerramentoPlanoAEECommand>
    {
        public NotificarEncerramentoPlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para notificação de seu encerramento.");

            RuleFor(a => a.Usuario)
                .NotEmpty()
                .WithMessage("O usuário deve ser informado para notificação do encerramento do plano AEE.");
        }
    }
}
