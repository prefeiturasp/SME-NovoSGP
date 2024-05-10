using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorDataTurmaComponenteCurricularCJQuery : IRequest<IEnumerable<AulaConsultaDto>>
    {
        public ObterAulasPorDataTurmaComponenteCurricularCJQuery(DateTime dataAula, string codigoTurma, long codigoComponenteCurricular, bool aulaCJ)
        {
            DataAula = dataAula;
            CodigoTurma = codigoTurma;
            CodigoComponenteCurricular = codigoComponenteCurricular;
            AulaCJ = aulaCJ;
        }

        public DateTime DataAula { get; private set; }
        public string CodigoTurma { get; private set; }
        public long CodigoComponenteCurricular { get; private set; }
        public bool AulaCJ { get; private set; }
    }

    public class ObterAulasPorDataTurmaComponenteCurricularCJQueryValidator : AbstractValidator<ObterAulasPorDataTurmaComponenteCurricularCJQuery>
    {
        public ObterAulasPorDataTurmaComponenteCurricularCJQueryValidator()
        {
            RuleFor(c => c.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada para a pesquisa de aulas.");

            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para a pesquisa de aulas.");

            RuleFor(c => c.CodigoComponenteCurricular)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado para a pesquisa de aulas.");
        }
    }
}
