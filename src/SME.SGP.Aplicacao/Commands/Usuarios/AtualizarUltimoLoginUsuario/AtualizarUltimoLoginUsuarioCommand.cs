using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AtualizarUltimoLoginUsuarioCommand : IRequest<bool>
    {
        public AtualizarUltimoLoginUsuarioCommand(Usuario usuario)
        {
            Usuario = usuario;
        }

        public Usuario Usuario { get; }
    }

    public class AtualizarUltimoLoginUsuarioCommandValidator : AbstractValidator<AtualizarUltimoLoginUsuarioCommand>
    {
        public AtualizarUltimoLoginUsuarioCommandValidator()
        {
            RuleFor(x => x.Usuario)
                .NotEmpty()
                .WithMessage("Entidade usuário deve ser informada para atualização de ultimo login");
        }
    }
}
