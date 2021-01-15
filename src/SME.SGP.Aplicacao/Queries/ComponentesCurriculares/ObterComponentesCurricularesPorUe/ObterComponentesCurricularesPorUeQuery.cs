using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUeQuery : IRequest<IEnumerable<ComponenteCurricularDto>>
    {
        public ObterComponentesCurricularesPorUeQuery(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }

        public string UeCodigo { get; set; }
    }

    public class ObterComponentesCurricularesPorUeQueryValidator : AbstractValidator<ObterComponentesCurricularesPorUeQuery>
    {
        public ObterComponentesCurricularesPorUeQueryValidator()
        {
            RuleFor(a => a.UeCodigo)
               .NotEmpty()
               .WithMessage("O código da UE deve ser informado para consulta de seus componentes curriculares.");
        }
    }
}
