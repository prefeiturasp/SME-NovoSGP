using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaPorIdCommand : IRequest<bool>
    {
        public ExcluirPendenciaPorIdCommand(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ExcluirPendenciaCommandValidator : AbstractValidator<ExcluirPendenciaPorIdCommand>
    {
        public ExcluirPendenciaCommandValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendência deve ser informado para exclusão da mesma.");
        }
    }
}
