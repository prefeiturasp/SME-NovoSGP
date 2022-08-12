using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaDevolutivaQueryValidator : AbstractValidator<ObterTurmaDaPendenciaDevolutivaQuery>
    {
        public ObterTurmaDaPendenciaDevolutivaQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
                .GreaterThan(0)
                .WithMessage("O id da pendencia deve ser informada.");
        }
    }
}
