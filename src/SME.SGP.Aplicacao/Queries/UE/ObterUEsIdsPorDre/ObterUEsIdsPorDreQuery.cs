using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsIdsPorDreQuery : IRequest<IEnumerable<long>>
    {

        public ObterUEsIdsPorDreQuery(long dreId)
        {
            DreId = dreId;
        }

        public long DreId { get; }
    }

    public class ObterUEsIdsPorDreQueryValidator : AbstractValidator<ObterUEsIdsPorDreQuery>
    {
        public ObterUEsIdsPorDreQueryValidator()
        {
            RuleFor(x => x.DreId)
                .NotEmpty()
                .WithMessage("O identificador da DRE deve ser informado para consultar as UEs");
        }
    }
}
