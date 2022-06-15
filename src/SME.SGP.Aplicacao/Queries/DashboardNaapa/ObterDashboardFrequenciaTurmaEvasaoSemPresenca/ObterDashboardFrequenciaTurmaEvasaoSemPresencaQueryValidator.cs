using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoSemPresencaQueryValidator : AbstractValidator<ObterDashboardFrequenciaTurmaEvasaoSemPresencaQuery>
    {
        public ObterDashboardFrequenciaTurmaEvasaoSemPresencaQueryValidator()
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
