using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasPorIdsQuery : IRequest<IEnumerable<DisciplinaDto>>
    {        
        public ObterDisciplinasPorIdsQuery(long[] ids)
        {
            Ids = ids;
        }

        public long[] Ids { get; set; }
    }

    public class ObterDisciplinasPorIdsQueryValidator : AbstractValidator<ObterDisciplinasPorIdsQuery>
    {
        public ObterDisciplinasPorIdsQueryValidator()
        {
            RuleFor(a => a.Ids)
                .NotEmpty()
                .WithMessage("Ids deve ser informado");
        }
    }

}
