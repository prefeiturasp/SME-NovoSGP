using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmaDreCommandValidator : AbstractValidator<ConciliacaoFrequenciaTurmaDreCommand>
    {
        public ConciliacaoFrequenciaTurmaDreCommandValidator()
        {
            RuleFor(c => c.DreId)
                .NotEmpty()
                .NotNull()
                .WithMessage("O ID da DRE deve ser informado.");

            RuleFor(a => a.AnoLetivo)
                .GreaterThanOrEqualTo(2020)
                .WithMessage("Um ano a partir de 2020 deve ser informado.");
        }
    }
}
