using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosPorTipoDeCalendarioDreUeQuery : IRequest<IEnumerable<Evento>>
    {
        public ObterEventosPorTipoDeCalendarioDreUeQuery(long tipoCalendarioId, string dreCodigo, string ueCodigo, bool ehEventoSME = false, bool filtroDreUe = true)
        {
            TipoCalendarioId = tipoCalendarioId;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            EhEventoSME = ehEventoSME;
            FiltroDreUe = filtroDreUe;
        }

        public long TipoCalendarioId { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public bool EhEventoSME { get; set; }
        public bool FiltroDreUe { get; set; }
    }

    public class ObterEventosPorTipoDeCalendarioDreUeQueryValidator : AbstractValidator<ObterEventosPorTipoDeCalendarioDreUeQuery>
    {
        public ObterEventosPorTipoDeCalendarioDreUeQueryValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
               .NotEmpty()
               .WithMessage("O id do tipo de calendário deve ser informado para pesquisa de seus eventos.");

        }
    }
}
