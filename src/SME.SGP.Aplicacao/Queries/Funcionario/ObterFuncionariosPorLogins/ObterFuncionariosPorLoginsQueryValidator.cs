using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorLoginsQueryValidator : AbstractValidator<ObterFuncionariosPorLoginsQuery>
    {
        public ObterFuncionariosPorLoginsQueryValidator()
        {
            RuleForEach(x => x.Logins)
                .NotEmpty()
                .WithMessage("A lista de logins deve ser informada.");
        }
    }
}
