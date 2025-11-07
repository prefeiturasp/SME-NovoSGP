using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ReiniciarSenhaCommand : IRequest<UsuarioReinicioSenhaDto>
    {
        public ReiniciarSenhaCommand(string codigoRf, string dreCodigo, string ueCodigo)
        {
            CodigoRf = codigoRf;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
        }

        public string CodigoRf { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
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
