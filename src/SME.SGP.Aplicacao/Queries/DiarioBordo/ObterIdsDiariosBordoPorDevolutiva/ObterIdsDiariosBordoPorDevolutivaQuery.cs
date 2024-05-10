using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsDiariosBordoPorDevolutivaQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsDiariosBordoPorDevolutivaQuery(long devolutivaId)
        {
            DevolutivaId = devolutivaId;
        }

        public long DevolutivaId { get; set; }
    }

    public  class ObterIdsDiariosBordoPorDevolutivaQueryValidator : AbstractValidator<ObterIdsDiariosBordoPorDevolutivaQuery>
    {
        public ObterIdsDiariosBordoPorDevolutivaQueryValidator()
        {
            RuleFor(a => a.DevolutivaId)
                .NotEmpty()
                .WithMessage("O id da devolutiva deve ser informado para consulta dos diários de bordo");
        }
    }
}
