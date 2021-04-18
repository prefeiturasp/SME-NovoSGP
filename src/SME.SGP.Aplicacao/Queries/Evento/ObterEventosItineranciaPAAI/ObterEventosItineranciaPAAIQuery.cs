using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosItineranciaPAAIQuery : IRequest<IEnumerable<EventoNomeDto>>
    {
        public ObterEventosItineranciaPAAIQuery(long tipoCalendarioId, string login, Guid perfil)
        {
            TipoCalendarioId = tipoCalendarioId;
            Login = login;
            Perfil = perfil;
        }

        public long TipoCalendarioId { get; }
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

            RuleFor(a => a.Login)
                .NotEmpty()
                .WithMessage("O RF do usuário deve ser informado para consulta de eventos de Itinerância PAAI");

            RuleFor(a => a.Perfil)
                .NotEmpty()
                .WithMessage("O perfil do usuário deve ser informado para consulta de eventos de Itinerância PAAI");
        }
    }
}
