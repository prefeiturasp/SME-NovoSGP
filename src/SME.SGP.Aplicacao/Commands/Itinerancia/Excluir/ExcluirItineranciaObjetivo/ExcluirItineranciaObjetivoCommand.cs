using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaObjetivoCommand : IRequest<bool>
    {
        public ExcluirItineranciaObjetivoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
    public class ExcluirItineranciaObjetivoCommandValidator : AbstractValidator<ExcluirItineranciaObjetivoCommand>
    {
        public ExcluirItineranciaObjetivoCommandValidator()
        {
            RuleFor(c => c.Id)
            .GreaterThan(0)
            .WithMessage("O id do objetivo da itinerância deve ser informado para exclusão.");
        }
    }
}
