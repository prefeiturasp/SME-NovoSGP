using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSinteseAlunoQuery : IRequest<SinteseDto>
    {
        public ObterSinteseAlunoQuery(double? percentualFrequencia, DisciplinaDto disciplina, int anoLetivo)
        {
            PercentualFrequencia = percentualFrequencia;
            Disciplina = disciplina;
            AnoLetivo = anoLetivo;
        }

        public double? PercentualFrequencia;
        public DisciplinaDto Disciplina;
        public int AnoLetivo;
    }

    public class ObterSinteseAlunoQueryValidator : AbstractValidator<ObterSinteseAlunoQuery>
    {
        public ObterSinteseAlunoQueryValidator()
        {
            RuleFor(a => a.PercentualFrequencia)
                .NotEmpty()
                .WithMessage("O percentual de frequência deve ser informado.");
            RuleFor(a => a.Disciplina)
                .NotEmpty()
                .WithMessage("A disciplina deve ser informada.");
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
