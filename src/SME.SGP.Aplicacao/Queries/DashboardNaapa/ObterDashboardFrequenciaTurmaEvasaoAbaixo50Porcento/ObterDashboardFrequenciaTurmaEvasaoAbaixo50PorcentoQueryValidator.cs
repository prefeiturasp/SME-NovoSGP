using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryValidator : AbstractValidator<ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery>
    {
        public ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Modalidade)
                .NotEmpty()
                .NotNull()
                .WithMessage("A modalidade deve ser informada.");
        }
    }
}
