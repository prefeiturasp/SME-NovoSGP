using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesRegenciaPorAnoEolQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesRegenciaPorAnoEolQuery(int anoTurma)
        {
            this.AnoTurma = anoTurma;
        }
        public int AnoTurma { get; set; }
    }
    
    public class ObterComponentesRegenciaPorAnoEolQueryValidator : AbstractValidator<ObterComponentesRegenciaPorAnoEolQuery>
    {
        public ObterComponentesRegenciaPorAnoEolQueryValidator()
        {

            RuleFor(c => c.AnoTurma)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O ano da turma deve ser informado para obter componentes curriculares por turma, login e perfil.");
        }
    }
}