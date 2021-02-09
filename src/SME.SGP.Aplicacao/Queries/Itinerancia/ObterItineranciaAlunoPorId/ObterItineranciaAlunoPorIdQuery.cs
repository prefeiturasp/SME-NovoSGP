using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciaAlunoPorIdQuery : IRequest<IEnumerable<ItineranciaAlunoDto>>
    {
        public ObterItineranciaAlunoPorIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }

    public class ObterItineranciaAlunoPorIdQueryValidator : AbstractValidator<ObterItineranciaAlunoPorIdQuery>
    {
        public ObterItineranciaAlunoPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O id da itinerancia deve ser informado.");

        }
    }
}
