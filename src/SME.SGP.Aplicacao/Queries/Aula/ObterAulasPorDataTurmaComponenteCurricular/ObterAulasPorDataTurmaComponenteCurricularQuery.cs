using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorDataTurmaComponenteCurricularQuery : IRequest<IEnumerable<AulaConsultaDto>>
    {
        public ObterAulasPorDataTurmaComponenteCurricularQuery(DateTime dataAula, string codigoTurma, long codigoComponenteCurricular, bool aulaCJ)
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

    public class ObterAulasPorDataTurmaComponenteCurricularQueryValidator : AbstractValidator<ObterAulasPorDataTurmaComponenteCurricularQuery>
    {
        public ObterAulasPorDataTurmaComponenteCurricularQueryValidator()
        {
            RuleFor(c => c.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada.");

            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.CodigoComponenteCurricular)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado.");
        }
    }
}
