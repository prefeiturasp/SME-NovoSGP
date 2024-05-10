using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorCpfQueryValidator : AbstractValidator<ObterUsuarioPorCpfQuery>
    {
        public ObterUsuarioPorCpfQueryValidator()
        {
            RuleFor(x => x.Cpf).NotEmpty().WithMessage("O CPF é Obrigátorio"); ;

        }
    }
}
