using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDescricaoComponentesCurricularesPorIdsQuery : IRequest<IEnumerable<ComponenteCurricularSimplesDto>>
    {
        public long[] Ids { get; }

        public ObterDescricaoComponentesCurricularesPorIdsQuery(long[] ids)
        {
            Ids = ids;
        }
    }

    public class ObterDescricaoComponentesCurricularesPorIdsQueryValidator : AbstractValidator<ObterDescricaoComponentesCurricularesPorIdsQuery>
    {
        public ObterDescricaoComponentesCurricularesPorIdsQueryValidator()
        {
            RuleFor(a => a.Ids)
                .NotEmpty()
                .WithMessage("Pelo menos um id deve ser informado para consulta da descrição do componente curricular");
        }
    }
}
