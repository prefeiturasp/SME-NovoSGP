using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{

    public class ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQueryValidator : AbstractValidator<ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQuery>
    {
        public ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQueryValidator()
        {
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado.");

            RuleFor(c => c.Bimestre)
               .NotNull()
               .WithMessage("O bimestre deve ser informado.");
        }
    }
}
