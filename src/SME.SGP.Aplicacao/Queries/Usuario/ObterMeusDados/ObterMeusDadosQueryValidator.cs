using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterMeusDadosQueryValidator : AbstractValidator<ObterMeusDadosQuery>
    {
        public ObterMeusDadosQueryValidator()
        {
            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("Login é obrigatório.");
        }
    }
}
