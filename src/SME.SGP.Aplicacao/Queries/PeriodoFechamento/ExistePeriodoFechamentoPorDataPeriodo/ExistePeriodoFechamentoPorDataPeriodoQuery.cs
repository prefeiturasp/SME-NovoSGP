using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExistePeriodoFechamentoPorDataPeriodoQuery : IRequest<bool>
    {
        public ExistePeriodoFechamentoPorDataPeriodoQuery(long periodoEscolarId, DateTime dataReferencia)
        {
            DataReferencia = dataReferencia;
            PeriodoEscolarId = periodoEscolarId;
        }

        public DateTime DataReferencia { get; set; }
        public long PeriodoEscolarId { get; set; }
    }

    public class ExistePeriodoFechamentoPorDataPeriodoQueryValidator : AbstractValidator<ExistePeriodoFechamentoPorDataPeriodoQuery>
    {
        public ExistePeriodoFechamentoPorDataPeriodoQueryValidator()
        {
            RuleFor(c => c.DataReferencia)
               .NotEmpty()
               .WithMessage("A data de referência deve ser informado para consulta de existencia de periodo de fechamento.");

            RuleFor(c => c.PeriodoEscolarId)
               .NotEmpty()
               .WithMessage("O id do periodo escolar deve ser informado para consulta de existencia de periodo de fechamento.");
        }
    }
}
