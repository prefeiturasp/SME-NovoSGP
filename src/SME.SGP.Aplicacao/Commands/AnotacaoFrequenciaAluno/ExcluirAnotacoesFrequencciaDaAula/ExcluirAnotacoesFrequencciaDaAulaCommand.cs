using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnotacoesFrequencciaDaAulaCommand : IRequest<bool>
    {
        public ExcluirAnotacoesFrequencciaDaAulaCommand(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ExcluirAnotacoesFrequencciaDaAulaCommandValidator : AbstractValidator<ExcluirAnotacoesFrequencciaDaAulaCommand>
    {
        public ExcluirAnotacoesFrequencciaDaAulaCommandValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O id da aula deve ser informado para exclusão de suas anotações de frequência");
        }
    }
}
