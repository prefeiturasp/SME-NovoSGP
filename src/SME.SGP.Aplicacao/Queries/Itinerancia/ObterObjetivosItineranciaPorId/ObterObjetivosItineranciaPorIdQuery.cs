using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosItineranciaPorIdQuery : IRequest<IEnumerable<ItineranciaObjetivoDto>>
    {
        public ObterObjetivosItineranciaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterObjetivosItineranciaPorIdQueryValidator : AbstractValidator<ObterObjetivosItineranciaPorIdQuery>
    {
        public ObterObjetivosItineranciaPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O id da itinerância deve ser informado.");

        }
    }
}
