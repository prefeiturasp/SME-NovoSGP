using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ReiniciarSenhaCommand : IRequest<UsuarioReinicioSenhaDto>
    {
        public ReiniciarSenhaCommand(string codigoRf)
        {
            CodigoRf = codigoRf;
        }

        public string CodigoRf { get; set; }
    }

    public class ReiniciarSenhaCommandValidator : AbstractValidator<ReiniciarSenhaCommand>
    {
        public ReiniciarSenhaCommandValidator()
        {
            RuleFor(c => c.CodigoRf)
                .NotEmpty()
                .WithMessage("O código Rf é obrigatório.");
        }
    }
}
