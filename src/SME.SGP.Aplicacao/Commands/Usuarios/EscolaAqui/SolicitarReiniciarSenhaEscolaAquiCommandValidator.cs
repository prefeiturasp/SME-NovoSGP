using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SolicitarReiniciarSenhaEscolaAquiCommandValidator : AbstractValidator<SolicitarReiniciarSenhaEscolaAquiCommand>
    {
        public SolicitarReiniciarSenhaEscolaAquiCommandValidator()
        {
            RuleFor(x => x.Cpf).NotEmpty().WithMessage("O CPF é Obrigátorio"); ;
        }
    }
}
