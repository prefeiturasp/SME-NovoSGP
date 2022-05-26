using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterListaNomePorListaLoginQueryValidator : AbstractValidator<ObterListaNomePorListaLoginQuery>
    {
        public ObterListaNomePorListaLoginQueryValidator()
        {
            RuleForEach(x => x.Logins)
                .NotEmpty()
                .WithMessage("A lista de logins deve ser informada.");
        }
    }
}
