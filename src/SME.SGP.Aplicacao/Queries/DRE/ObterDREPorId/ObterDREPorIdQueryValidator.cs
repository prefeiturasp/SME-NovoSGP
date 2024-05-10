using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterDREPorIdQueryValidator : AbstractValidator<ObterDREPorIdQuery>
    {
        public ObterDREPorIdQueryValidator()
        {
            RuleFor(a => a.DreId)
                .NotEmpty()
                .WithMessage("O Id da Dre deve ser informado.");
        }
    }
}
