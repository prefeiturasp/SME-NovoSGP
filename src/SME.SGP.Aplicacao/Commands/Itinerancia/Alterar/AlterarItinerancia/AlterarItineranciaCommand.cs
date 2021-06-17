using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarItineranciaCommand : IRequest<AuditoriaDto>
    {
        public AlterarItineranciaCommand(Itinerancia itinerancia)
        {
            this.itinerancia = itinerancia;
        }

        public Itinerancia itinerancia { get; set; }

        
    }
    public class AlterarItineranciaCommandValidator : AbstractValidator<AlterarItineranciaCommand>
    {
        public AlterarItineranciaCommandValidator()
        {
            RuleFor(x => x.itinerancia)
                   .NotEmpty()
                   .WithMessage("O Id da itinerância deve ser informado!");
        }
    }
}
