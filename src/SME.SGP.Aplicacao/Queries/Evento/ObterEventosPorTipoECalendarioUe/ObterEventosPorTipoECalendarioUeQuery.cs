using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosPorTipoECalendarioUeQuery : IRequest<IEnumerable<Evento>>
    {
        public ObterEventosPorTipoECalendarioUeQuery(long tipoCalendarioId, string ueCodigo, TipoEvento tipoEvento)
        {
            TipoCalendarioId = tipoCalendarioId;
            UeCodigo = ueCodigo;
            TipoEvento = tipoEvento;
        }

        public long TipoCalendarioId { get; set; }
        public string UeCodigo { get; set; }
        public TipoEvento TipoEvento { get; set; }
    }

    public class ObterEventosPorCalendarioUeQueryValidator : AbstractValidator<ObterEventosPorTipoECalendarioUeQuery>
    {
        public ObterEventosPorCalendarioUeQueryValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
            .NotEmpty()
            .WithMessage("O id do tipo de calendário deve ser informado para consulta dos eventos no calendário.");

            RuleFor(c => c.UeCodigo)
            .NotEmpty()
            .WithMessage("O código da UE deve ser informado para consulta dos eventos no calendário.");

            RuleFor(c => c.TipoEvento)
            .NotEmpty() 
            .WithMessage("O tipo de evento deve ser informado para consulta dos eventos no calendário.");
        }
    }
}
