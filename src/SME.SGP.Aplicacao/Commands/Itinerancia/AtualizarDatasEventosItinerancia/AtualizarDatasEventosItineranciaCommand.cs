using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AtualizarDatasEventosItineranciaCommand : IRequest<bool>
    {
        public AtualizarDatasEventosItineranciaCommand(long itineranciaId, DateTime dataEvento)
        {
            ItineranciaId = itineranciaId;
            DataEvento = dataEvento;
        }

        public long ItineranciaId { get; }
        public DateTime DataEvento { get; }
    }

    public class AtualizarDatasEventosItineranciaCommandValidator : AbstractValidator<AtualizarDatasEventosItineranciaCommand>
    {
        public AtualizarDatasEventosItineranciaCommandValidator()
        {
            RuleFor(a => a.ItineranciaId)
                .NotEmpty()
                .WithMessage("O id da itinerância deve ser informado para atualização das datas dos eventos");

            RuleFor(a => a.DataEvento)
                .NotEmpty()
                .WithMessage("A data do evento deve ser informada para atualização dos eventos da itinerância");
        }
    }
}
