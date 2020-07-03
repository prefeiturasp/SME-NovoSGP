using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFrequenciaDaAulaCommand: IRequest<bool>
    {
        public ExcluirFrequenciaDaAulaCommand(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ExcluirFrequenciaDaAulaCommandValidator: AbstractValidator<ExcluirFrequenciaDaAulaCommand>
    {
        public ExcluirFrequenciaDaAulaCommandValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O Id da aula deve ser informado para exclusão de seu registro de frequência.");
        }
    }
}
