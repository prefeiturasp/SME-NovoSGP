using FluentValidation;

namespace SME.SGP.Aplicacao.Queries.EOL.Usuarios.ObterUsuariosCoreSsoPorRf
{
    public class ObterUsuariosCoreSsoPorRfQueryValidator : AbstractValidator<ObterUsuariosCoreSsoPorRfQuery>
    {
        public ObterUsuariosCoreSsoPorRfQueryValidator()
        {
            RuleFor(c => c.CodigoRf)
                .NotEmpty()
                .WithMessage("O código RF do usuário é obrigatório.");
        }
    }
}