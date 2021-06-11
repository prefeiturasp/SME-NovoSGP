using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class RemoverEventosItineranciaCommand : IRequest<bool>
    {
        public RemoverEventosItineranciaCommand(long itineranciaId)
        {
            ItineranciaId = itineranciaId;
        }

        public long ItineranciaId { get; }
    }

    public class RemoverEventosItineranciaCommandValidator : AbstractValidator<RemoverEventosItineranciaCommand>
    {
        public RemoverEventosItineranciaCommandValidator()
        {
            RuleFor(a => a.ItineranciaId)
                .NotEmpty()
                .WithMessage("O id da itinerância deve ser informado para exclusão de seus eventos");
        }
    }
}
