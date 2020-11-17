using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirTodasPendenciasAulaCommand : IRequest<bool>
    {
        public ExcluirTodasPendenciasAulaCommand(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ExcluirTodasPendenciasAulaCommandValidator : AbstractValidator<ExcluirTodasPendenciasAulaCommand>
    {
        public ExcluirTodasPendenciasAulaCommandValidator()
        {
            RuleFor(c => c.AulaId)
            .NotEmpty()
            .WithMessage("O id da aula deve ser informado para exclusão de todas suas pendências.");
        }
    }
}
