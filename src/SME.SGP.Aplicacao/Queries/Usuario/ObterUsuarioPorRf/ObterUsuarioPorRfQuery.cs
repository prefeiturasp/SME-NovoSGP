using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorRfQuery : IRequest<Dominio.Usuario>
    {
        public ObterUsuarioPorRfQuery(string codigoRf)
        {
            CodigoRf = codigoRf;
        }

        public string CodigoRf { get; set; }
    }

    public class ObterUsuarioPorRfQueryValidator : AbstractValidator<ObterUsuarioPorRfQuery>
    {
        public ObterUsuarioPorRfQueryValidator()
        {
            RuleFor(c => c.CodigoRf)
                .NotEmpty()
                .WithMessage("O código RF deve ser informado.");
        }
    }
}
