using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }

    public class ObterComponentesCurricularesRegenciaPorTurmaCodigoQueryValidator : AbstractValidator<ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery>
    {
        public ObterComponentesCurricularesRegenciaPorTurmaCodigoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado");
        }
    }

}
