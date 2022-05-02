using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoSemPresencaQueryValidator : AbstractValidator<ObterDashboardFrequenciaTurmaEvasaoSemPresencaQuery>
    {
        public ObterDashboardFrequenciaTurmaEvasaoSemPresencaQueryValidator()
        {
            RuleFor(c => c.Modalidade)
                .NotEmpty()
                .NotNull()
                .WithMessage("A modalidade deve ser informada.");
        }
    }
}
