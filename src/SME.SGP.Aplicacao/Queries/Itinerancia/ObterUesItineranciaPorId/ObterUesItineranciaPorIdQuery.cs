using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUesItineranciaPorIdQuery : IRequest<IEnumerable<ItineranciaUeDto>>
    {
        public ObterUesItineranciaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterUesItineranciaPorIdQueryValidator : AbstractValidator<ObterUesItineranciaPorIdQuery>
    {
        public ObterUesItineranciaPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O id da itinerância deve ser informado.");

        }
    }
}
