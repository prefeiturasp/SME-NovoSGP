using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciaPorIdQuery : IRequest<Itinerancia>
    {
        public ObterItineranciaPorIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }

    public class ObterItineranciaPorIdQueryValidator : AbstractValidator<ObterItineranciaPorIdQuery>
    {
        public ObterItineranciaPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O id da itinerância deve ser informado.");

        }
    }
}
