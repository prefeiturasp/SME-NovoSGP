using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class AlterarSituacaoItineranciaCommandValidator : AbstractValidator<AlterarSituacaoItineranciaCommand>
    {
        public AlterarSituacaoItineranciaCommandValidator()
        {
            RuleFor(x => x.ItineranciaId)
                   .NotEmpty()
                   .WithMessage("O Id da itinerância deve ser informado!");
            RuleFor(x => x.Situacao)
                   .NotEmpty()
                   .WithMessage("A situação deve ser informada!");
        }
    }
}
