using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPorDataAulasExistentesQuery : IRequest<IEnumerable<DateTime>>
    {
        public List<DateTime> DiasParaIncluirRecorrencia { get; set; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public bool ProfessorCJ { get; set; }

        public ObterAulaPorDataAulasExistentesQuery(List<DateTime> diasParaIncluirRecorrencia, string turmaCodigo, string componenteCurricularCodigo, bool professorCJ)
        {
            DiasParaIncluirRecorrencia = diasParaIncluirRecorrencia;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            ProfessorCJ = professorCJ;
        }
    }

    public class ObterAulaPorDataAulasExistentesQueryValidator : AbstractValidator<ObterAulaPorDataAulasExistentesQuery>
    {
        public ObterAulaPorDataAulasExistentesQueryValidator()
        {
            RuleFor(x => x.DiasParaIncluirRecorrencia)
                .NotEmpty()
                .WithMessage("As datas para inclusão de recorrência deve ser informadas.");

            RuleFor(x => x.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(x => x.ComponenteCurricularCodigo)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado.");
        }
    }
}