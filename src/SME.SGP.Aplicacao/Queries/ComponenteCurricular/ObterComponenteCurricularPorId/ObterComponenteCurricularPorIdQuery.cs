using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteCurricularPorIdQuery : IRequest<DisciplinaDto>
    {
        public ObterComponenteCurricularPorIdQuery(long componenteCurricularId)
        {
            ComponenteCurricularId = componenteCurricularId;
        }

        public long ComponenteCurricularId { get; set; }
    }

    public class ObterComponenteCurricularPorIdQueryValidator : AbstractValidator<ObterComponenteCurricularPorIdQuery>
    {
        public ObterComponenteCurricularPorIdQueryValidator()
        {
            RuleFor(c => c.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O id do componente curricular deve ser informado.");
        }
    }
}
