using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosPorTipoCalendarioQuery : IRequest<IEnumerable<Evento>>
    {
        public ObterEventosPorTipoCalendarioQuery(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }
        public long TipoCalendarioId { get; set; }
    }

    public class ObterEventosPorTipoCalendarioQueryValidator : AbstractValidator<ObterEventosPorTipoCalendarioQuery>
    {
        public ObterEventosPorTipoCalendarioQueryValidator()
        {
            RuleFor(c => c.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado.");
        }
    }
}
