using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterEventoFechamenoPorIdQuery : IRequest<EventoFechamento>
    {
        public long Id { get; set; }

        public ObterEventoFechamenoPorIdQuery(long id)
        {
            Id = id;
        }
    }

    public class ObterEventoFechamenoPorIdQueryValidator : AbstractValidator<ObterEventoFechamenoPorIdQuery>
    {
        public ObterEventoFechamenoPorIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O id do evento fechamento deve ser informado.");           
        }
    }
}