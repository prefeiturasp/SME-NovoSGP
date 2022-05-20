using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaAnoCommandValidator : AbstractValidator<ConciliacaoFrequenciaAnoCommand>
    {
        public ConciliacaoFrequenciaAnoCommandValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .GreaterThanOrEqualTo(2020)
                .WithMessage("Um ano a partir de 2020 deve ser informado.");
        }
    }
}
