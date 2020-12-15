using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTurmasSeriadasNaUeQuery : IRequest<int>
    {
        public ObterQuantidadeTurmasSeriadasNaUeQuery(long ueId, int ano)
        {
            UeId = ueId;
            Ano = ano;
        }

        public long UeId { get; set; }
        public int Ano { get; set; }
    }

    public class ObterQuantidadeTurmasSeriadasNaUeQueryValidator : AbstractValidator<ObterQuantidadeTurmasSeriadasNaUeQuery>
    {
        public ObterQuantidadeTurmasSeriadasNaUeQueryValidator()
        {
            RuleFor(c => c.UeId)
               .Must(a => a > 0)
               .WithMessage("O id da UE deve ser informado para consulta de quantidade de turmas seriadas.");

            RuleFor(c => c.Ano)
               .Must(a => a > 0)
               .WithMessage("O ano letivo deve ser informado para consulta de quantidade de turmas seriadas.");
        }
    }
}
