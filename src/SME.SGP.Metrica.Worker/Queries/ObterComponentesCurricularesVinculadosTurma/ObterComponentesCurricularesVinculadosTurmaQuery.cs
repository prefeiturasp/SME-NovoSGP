using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Metrica.Worker.Queries
{
    public class ObterComponentesCurricularesVinculadosTurmaQuery : IRequest<IEnumerable<string>>
    {
        public ObterComponentesCurricularesVinculadosTurmaQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; }
    }

    public class ObterComponentesCurricularesVinculadosTurmaQueryValidator : AbstractValidator<ObterComponentesCurricularesVinculadosTurmaQuery>
    {
        public ObterComponentesCurricularesVinculadosTurmaQueryValidator()
        {
            RuleFor(a => a.CodigoTurma)
                .NotEmpty()
                .WithMessage("Necessário informar o código de turma para consulta de componentes curriculares.");
        }
    }
}
