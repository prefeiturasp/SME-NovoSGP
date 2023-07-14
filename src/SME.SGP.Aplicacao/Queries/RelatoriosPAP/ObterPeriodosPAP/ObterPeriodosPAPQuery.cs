using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosPAPQuery : IRequest<IEnumerable<PeriodosPAPDto>>
    {
        public ObterPeriodosPAPQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; set; }
    }

    public class ObterPeriodosPAPQueryValidator : AbstractValidator<ObterPeriodosPAPQuery>
    {
        public ObterPeriodosPAPQueryValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }
}
