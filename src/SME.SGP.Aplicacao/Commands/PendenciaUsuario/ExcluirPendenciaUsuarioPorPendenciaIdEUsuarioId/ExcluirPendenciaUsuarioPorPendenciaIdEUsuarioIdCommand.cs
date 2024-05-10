using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommand : IRequest<bool>
    {
        public ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommand(long pendenciaId, long usuarioId)
        {
            PendenciaId = pendenciaId;
            UsuarioId = usuarioId;
        }

        public long PendenciaId { get; set; }
        public long UsuarioId { get; set; }
    }

    public class ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommandValidator : AbstractValidator<ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommand>
    {
        public ExcluirPendenciaUsuarioPorPendenciaIdEUsuarioIdCommandValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendência deve ser informado para exclusão de pendência usuário.");

            RuleFor(c => c.UsuarioId)
                .NotEmpty()
                .WithMessage("O id do usuário deve ser informado para exclusão de pendência usuário.");
        }
    }
}
