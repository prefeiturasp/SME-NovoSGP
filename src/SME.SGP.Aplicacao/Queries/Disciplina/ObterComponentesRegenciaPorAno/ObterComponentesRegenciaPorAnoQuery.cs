using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesRegenciaPorAnoQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesRegenciaPorAnoQuery(int anoTurma)
        {
            this.AnoTurma = anoTurma;
        }
        public int AnoTurma { get; set; }
    }
    
    public class ObterComponentesRegenciaPorAnoQueryValidator : AbstractValidator<ObterComponentesRegenciaPorAnoQuery>
    {
        public ObterComponentesRegenciaPorAnoQueryValidator()
        {

            RuleFor(c => c.AnoTurma)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O ano da turma deve ser informado para obter componentes curriculares por turma, login e perfil.");
        }
    }
}