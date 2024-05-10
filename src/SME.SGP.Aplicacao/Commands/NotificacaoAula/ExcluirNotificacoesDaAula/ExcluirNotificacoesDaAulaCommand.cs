using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacoesDaAulaCommand: IRequest<bool>
    {
        public ExcluirNotificacoesDaAulaCommand(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ExcluirNotificacoesDaAulaCommandValidator: AbstractValidator<ExcluirNotificacoesDaAulaCommand>
    {
        public ExcluirNotificacoesDaAulaCommandValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("É necessário informar o Id da aula para exclusão de suas notificações");
        }
    }
}
