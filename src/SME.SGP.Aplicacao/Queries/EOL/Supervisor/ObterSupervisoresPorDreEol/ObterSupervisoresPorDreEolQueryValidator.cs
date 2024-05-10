using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorDreEolQueryValidator : AbstractValidator<ObterSupervisoresPorDreEolQuery>
    {
        public ObterSupervisoresPorDreEolQueryValidator()
        {
            RuleFor(c => c.DreCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O código da Dre deve ser informado.");
        }
    }
}
