using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaPerfilUsuarioCommand : IRequest
    {
        public ExcluirPendenciaPerfilUsuarioCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ExcluirPendenciaPerfilUsuarioCommandValidator : AbstractValidator<ExcluirPendenciaPerfilUsuarioCommand>
    {
        public ExcluirPendenciaPerfilUsuarioCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id de Pendencia Perfil Usuário deve ser informado para a exclusão.");
        }
    }
}
