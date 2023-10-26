using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery : IRequest<IEnumerable<AulaConsultaDto>>
    {
        public ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery(DateTime dataAula, string codigoTurma, long[] codigosComponentesCurriculares, string codigoRfProfessor = null)
        {
            DataAula = dataAula;
            CodigoTurma = codigoTurma;
            CodigosComponentesCurriculares = codigosComponentesCurriculares;
            CodigoRfProfessor = codigoRfProfessor;
        }

        public DateTime DataAula { get; private set; }
        public string CodigoTurma { get; private set; }
        public long[] CodigosComponentesCurriculares { get; private set; }
        public string CodigoRfProfessor { get; private set; }
    }

    public class ObterAulasPorDataTurmaComponenteCurricularEProfessorQueryValidator : AbstractValidator<ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery>
    {
        public ObterAulasPorDataTurmaComponenteCurricularEProfessorQueryValidator()
        {
            RuleFor(c => c.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada.");

            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.CodigosComponentesCurriculares)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado.");

        }
    }
}
