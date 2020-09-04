using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SolicitarReiniciarSenhaCommandValidator : AbstractValidator<SolicitarReiniciarSenhaCommand>
    {
        public SolicitarReiniciarSenhaCommandValidator()
        {
            RuleFor(x => x.Cpf).NotEmpty().WithMessage("O CPF é Obrigátorio"); ;
        }
    }
}
