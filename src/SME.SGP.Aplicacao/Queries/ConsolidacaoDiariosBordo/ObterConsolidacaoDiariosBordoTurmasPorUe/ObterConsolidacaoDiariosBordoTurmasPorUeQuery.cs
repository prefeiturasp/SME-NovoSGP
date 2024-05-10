using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoDiariosBordoTurmasPorUeQuery : IRequest<IEnumerable<ConsolidacaoDiariosBordo>>
    {
        public ObterConsolidacaoDiariosBordoTurmasPorUeQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; }
    }

    public class ObterConsolidacaoDiariosBordoTurmasPorUeQueryValidator : AbstractValidator<ObterConsolidacaoDiariosBordoTurmasPorUeQuery>
    {
        public ObterConsolidacaoDiariosBordoTurmasPorUeQueryValidator()
        {
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O identificador da UE deve ser informado para consolidar os diarios de bordo de suas turmas");

        }
    }
}
