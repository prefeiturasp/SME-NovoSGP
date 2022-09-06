using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesSimplesPorIdsQuery : IRequest<IEnumerable<ComponenteCurricularSimplesDto>>
    {
        public long[] Ids { get; }

        public ObterComponentesCurricularesSimplesPorIdsQuery(long[] ids)
        {
            Ids = ids;
        }
    }

    public class ObterDescPermLanctoNotaComponentesCurricularesPorIdsQueryValidator : AbstractValidator<ObterComponentesCurricularesSimplesPorIdsQuery>
    {
        public ObterDescPermLanctoNotaComponentesCurricularesPorIdsQueryValidator()
        {
            RuleFor(a => a.Ids)
                .NotEmpty()
                .WithMessage("Pelo menos um id deve ser informado para consulta da descrição do componente curricular");
        }
    }
}
