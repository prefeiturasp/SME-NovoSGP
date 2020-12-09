using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTurmasSeriadasNaUeQuery : IRequest<int>
    {
        public ObterQuantidadeTurmasSeriadasNaUeQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }

    public class ObterQuantidadeTurmasSeriadasNaUeQueryValidator : AbstractValidator<ObterQuantidadeTurmasSeriadasNaUeQuery>
    {
        public ObterQuantidadeTurmasSeriadasNaUeQueryValidator()
        {
            RuleFor(c => c.UeId)
               .Must(a => a > 0)
               .WithMessage("O id da UE deve ser informado para consulta de quantidade de turmas seriadas.");
        }
    }
}
