using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUesPorIdsQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUesPorIdsQuery(long[] ids)
        {
            Ids = ids;
        }

        public long[] Ids { get; set; }
    }
    public class ObterUesPorIdsQueryValidator : AbstractValidator<ObterUesPorIdsQuery>
    {
        public ObterUesPorIdsQueryValidator()
        {
            RuleFor(c => c.Ids)
            .NotEmpty()
            .WithMessage("Os ids devem ser informados para consulta das UEs.");
        }
    }
}
