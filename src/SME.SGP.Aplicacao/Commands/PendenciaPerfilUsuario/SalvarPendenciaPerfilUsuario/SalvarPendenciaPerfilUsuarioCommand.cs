using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaPerfilUsuarioCommand : IRequest
    {
        public SalvarPendenciaPerfilUsuarioCommand(long pendenciaPerfilId, long usuarioId, PerfilUsuario perfilCodigo)
        {
            PendenciaPerfilId = pendenciaPerfilId;
            UsuarioId = usuarioId;
            PerfilCodigo = perfilCodigo;
        }

        public long PendenciaPerfilId { get; }
        public long UsuarioId { get; }
        public PerfilUsuario PerfilCodigo { get; }
    }

    public class SalvarPendenciaPerfilUsuarioCommandValidator : AbstractValidator<SalvarPendenciaPerfilUsuarioCommand>
    {
        public SalvarPendenciaPerfilUsuarioCommandValidator()
        {
            RuleFor(a => a.PendenciaPerfilId)
                .NotEmpty()
                .WithMessage("O id da relação Pendencia x Perfil deve ser informado para relacionar a um usuário");

            RuleFor(a => a.UsuarioId)
                .NotEmpty()
                .WithMessage("O id do usuário deve ser informado para relacionar a uma Pendência x Perfil");

            RuleFor(a => a.PerfilCodigo)
                .NotEmpty()
                .WithMessage("O código do perfil deve ser informado para relacionar uma Pendência x Perfil ao usuário");
        }
    }
}
