using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class NotificarEventoCommand : IRequest<bool>
    {
        public NotificarEventoCommand(Evento evento)
        {
            Evento = evento;
        }

        public Evento Evento { get; set; }
    }

    public class NotificarEventoCommandValidator : AbstractValidator<NotificarEventoCommand>
    {
        public NotificarEventoCommandValidator()
        {
            RuleFor(c => c.Evento)
               .NotEmpty()
               .WithMessage("O evento deve ser informado para notificação.");

        }
    }
}
