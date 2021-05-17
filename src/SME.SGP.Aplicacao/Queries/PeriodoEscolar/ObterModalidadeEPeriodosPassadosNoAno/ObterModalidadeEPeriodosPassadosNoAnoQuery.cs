using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadeEPeriodosPassadosNoAnoQuery : IRequest<IEnumerable<PeriodoEscolarModalidadeDto>>
    {
        public ObterModalidadeEPeriodosPassadosNoAnoQuery(System.DateTime data)
        {
            Data = data;
        }

        public DateTime Data { get; }
    }

    public class ObterModalidadeEPeriodosPassadosNoAnoQueryValidator : AbstractValidator<ObterModalidadeEPeriodosPassadosNoAnoQuery>
    {
        public ObterModalidadeEPeriodosPassadosNoAnoQueryValidator()
        {
            RuleFor(a => a.Data)
                .NotEmpty()
                .WithMessage("A data deve ser informada para obter periodos escolares passados");
        }
    }
}
