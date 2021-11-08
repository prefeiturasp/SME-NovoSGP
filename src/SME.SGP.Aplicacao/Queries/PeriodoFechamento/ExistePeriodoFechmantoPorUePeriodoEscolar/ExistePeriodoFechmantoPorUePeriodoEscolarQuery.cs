using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExistePeriodoFechmantoPorUePeriodoEscolarQuery : IRequest<bool>
    {
        public ExistePeriodoFechmantoPorUePeriodoEscolarQuery(long periodoEscolarId, DateTime dataReferencia)
        {
            DataReferencia = dataReferencia;
            PeriodoEscolarId = periodoEscolarId;
        }

        public DateTime DataReferencia { get; set; }
        public long PeriodoEscolarId { get; set; }
    }

    public class ExistePeriodoFechmantoPorUePeriodoEscolarQueryValidator : AbstractValidator<ExistePeriodoFechmantoPorUePeriodoEscolarQuery>
    {
        public ExistePeriodoFechmantoPorUePeriodoEscolarQueryValidator()
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
