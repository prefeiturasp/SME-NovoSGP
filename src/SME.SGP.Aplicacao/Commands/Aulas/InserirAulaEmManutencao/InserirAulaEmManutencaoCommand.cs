using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class InserirAulaEmManutencaoCommand: IRequest<ProcessoExecutando>
    {
        public InserirAulaEmManutencaoCommand(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class InserirAulaEmManutencaoCommandValidator: AbstractValidator<InserirAulaEmManutencaoCommand>
    {
        public InserirAulaEmManutencaoCommandValidator()
        {
            RuleFor(c => c.AulaId)
                .NotEmpty()
                .WithMessage("Necessário informar o id da aula para incluir o registro em manutenção.");
        }
    }
}
