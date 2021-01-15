using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaParametroEventoCommand : IRequest<bool>
    {
        public ExcluirPendenciaParametroEventoCommand(PendenciaParametroEvento pendenciaParametroEvento)
        {
            PendenciaParametroEvento = pendenciaParametroEvento;
        }

        public PendenciaParametroEvento PendenciaParametroEvento { get; set; }
    }

    public class ExcluirPendenciaParametroEventoCommandValidator : AbstractValidator<ExcluirPendenciaParametroEventoCommand>
    {
        public ExcluirPendenciaParametroEventoCommandValidator()
        {
            RuleFor(c => c.PendenciaParametroEvento)
               .NotEmpty()
               .WithMessage("A pendencia parametro deve ser informada para exclusão.");
        }
    }
}
