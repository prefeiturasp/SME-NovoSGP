using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AtualizarPendenciaParametroEventoCommand : IRequest<bool>
    {
        public AtualizarPendenciaParametroEventoCommand(PendenciaParametroEvento pendenciaParametroEvento)
        {
            PendenciaParametroEvento = pendenciaParametroEvento;
        }

        public PendenciaParametroEvento PendenciaParametroEvento { get; set; }
    }

    public class AtualizarPendenciaParametroEventoCommandValidator : AbstractValidator<AtualizarPendenciaParametroEventoCommand>
    {
        public AtualizarPendenciaParametroEventoCommandValidator()
        {
            RuleFor(c => c.PendenciaParametroEvento)
               .NotEmpty()
               .WithMessage("A pendência parametro evento deve ser informada para atualização.");

        }
    }
}
