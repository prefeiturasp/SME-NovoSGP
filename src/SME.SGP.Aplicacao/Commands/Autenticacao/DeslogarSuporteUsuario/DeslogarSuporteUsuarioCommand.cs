using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Commands.Autenticacao.DeslogarSuporteUsuario
{
    public class DeslogarSuporteUsuarioCommand : IRequest<UsuarioAutenticacaoRetornoDto>
    {
        public DeslogarSuporteUsuarioCommand(AdministradorSuporte administradorSuporte)
        {
            Administrador = administradorSuporte;
        }

        public AdministradorSuporte Administrador { get; private set; }
    }

    public class DeslogarSuporteUsuarioCommandValidator : AbstractValidator<DeslogarSuporteUsuarioCommand>
    {
        public DeslogarSuporteUsuarioCommandValidator()
        {
            RuleFor(deslogar => deslogar.Administrador)
            .NotEmpty()
            .WithMessage("O administrador deve ser informado.");
        }
    }
}
