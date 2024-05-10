using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaObjetivoCommand : IRequest<bool>
    {
        public ExcluirItineranciaObjetivoCommand(long objetivoId, long itineranciaId)
        {
            ObjetivoId = objetivoId;
            ItineranciaId = itineranciaId;
        }

        public long ObjetivoId { get; set; }
        public long ItineranciaId { get; set; }
    }
    public class ExcluirItineranciaObjetivoCommandValidator : AbstractValidator<ExcluirItineranciaObjetivoCommand>
    {
        public ExcluirItineranciaObjetivoCommandValidator()
        {
            RuleFor(c => c.ObjetivoId)
            .GreaterThan(0)
            .WithMessage("O id do objetivo da itinerância deve ser informado para exclusão.");

            RuleFor(c => c.ItineranciaId)
            .GreaterThan(0)
            .WithMessage("O id da itinerância deve ser informado para exclusão.");
        }
    }
}
