using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasUsuariosPorPendenciaIdCommand : IRequest<bool>
    {
        public ExcluirPendenciasUsuariosPorPendenciaIdCommand(long pendenciaPerfilId)
        {
            PendenciaPerfilId = pendenciaPerfilId;
        }

        public long PendenciaPerfilId { get; set; }
    }

    public class ExcluirPendenciasUsuariosPorPendenciaIdCommandValidator : AbstractValidator<ExcluirPendenciasUsuariosPorPendenciaIdCommand>
    {
        public ExcluirPendenciasUsuariosPorPendenciaIdCommandValidator()
        {
            RuleFor(c => c.PendenciaPerfilId)
            .NotEmpty()
            .WithMessage("O id da pendência deve ser informado para exclusão de pendência usuário.");
        }
    }
}
