using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class VerificarComponenteCurriculareSeERegenciaPorIdQuery : IRequest<bool>
    {
        public VerificarComponenteCurriculareSeERegenciaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class VerificarComponentesCurricularesRegenciaPorIdQueryValidator : AbstractValidator<VerificarComponenteCurriculareSeERegenciaPorIdQuery>
    {
        public VerificarComponentesCurricularesRegenciaPorIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id do componente curricular deve ser informado");
        }
    }
}
