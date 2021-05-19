using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosItineranciaPAAIQuery : IRequest<IEnumerable<EventoNomeDto>>
    {
        public ObterEventosItineranciaPAAIQuery(long tipoCalendarioId, long itineranciaId, string codigoUE, string login, Guid perfil)
        {
            TipoCalendarioId = tipoCalendarioId;
            ItineranciaId = itineranciaId;
            Login = login;
            Perfil = perfil;
            CodigoUE = codigoUE;
        }

        public long TipoCalendarioId { get; }
        public long ItineranciaId { get; }
        public string CodigoUE { get; set; }
        public string Login { get; }
        public Guid Perfil { get; }
    }

    public class ObterEventosItineranciaPAAIQueryValidator : AbstractValidator<ObterEventosItineranciaPAAIQuery>
    {
        public ObterEventosItineranciaPAAIQueryValidator()
        {
            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O id do tipo de calendário deve ser informado para consulta de eventos de Itinerância PAAI");

            RuleFor(a => a.CodigoUE)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para consulta de eventos de Itinerância PAAI");

            RuleFor(a => a.Login)
                .NotEmpty()
                .WithMessage("O RF do usuário deve ser informado para consulta de eventos de Itinerância PAAI");

            RuleFor(a => a.Perfil)
                .NotEmpty()
                .WithMessage("O perfil do usuário deve ser informado para consulta de eventos de Itinerância PAAI");
        }
    }
}
