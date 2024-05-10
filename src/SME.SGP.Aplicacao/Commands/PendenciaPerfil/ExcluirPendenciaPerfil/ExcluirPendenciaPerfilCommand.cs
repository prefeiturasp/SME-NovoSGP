using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaPerfilCommand : IRequest<bool>
    {
        public long PendenciaId { get; set; }

        public ExcluirPendenciaPerfilCommand(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }
    }

    public class ExcluirPendenciaPerfilCommandValidador : AbstractValidator<ExcluirPendenciaPerfilCommand>
    {
        public ExcluirPendenciaPerfilCommandValidador()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id de pendência deve ser informado para geração de pendência.");
        }
    }
}
