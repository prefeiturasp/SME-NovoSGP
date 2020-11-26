using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaFechamentoCommand : IRequest<bool>
    {
        public ExcluirPendenciaFechamentoCommand(PendenciaFechamento pendenciaFechamento)
        {
            PendenciaFechamento = pendenciaFechamento;
        }

        public PendenciaFechamento PendenciaFechamento { get; set; }
    }

    public class ExcluirPendenciaFechamentoCommandValidator : AbstractValidator<ExcluirPendenciaFechamentoCommand>
    {
        public ExcluirPendenciaFechamentoCommandValidator()
        {
            RuleFor(c => c.PendenciaFechamento)
               .NotEmpty()
               .WithMessage("A pendencia do fechamento deve ser informada para sua exclusão.");
        }
    }
}
