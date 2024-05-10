using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasForaDoPeriodoEscolarQuery : IRequest<IEnumerable<DateTime>>
    {
        public ObterDiasForaDoPeriodoEscolarQuery(IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            PeriodosEscolares = periodosEscolares;
        }

        public ObterDiasForaDoPeriodoEscolarQuery(PeriodoEscolar periodoEscolar)
        {
            PeriodosEscolares = new List<PeriodoEscolar> { periodoEscolar };
        }

        public IEnumerable<PeriodoEscolar> PeriodosEscolares { get; set; }
    }

    public class ObterDiasForaDoPeriodoEscolarQueryValidator : AbstractValidator<ObterDiasForaDoPeriodoEscolarQuery>
    {
        public ObterDiasForaDoPeriodoEscolarQueryValidator()
        {
            RuleFor(c => c.PeriodosEscolares)
                .NotEmpty()
                .WithMessage("Os periodos escolares devem ser informados.");
        }
    }
}