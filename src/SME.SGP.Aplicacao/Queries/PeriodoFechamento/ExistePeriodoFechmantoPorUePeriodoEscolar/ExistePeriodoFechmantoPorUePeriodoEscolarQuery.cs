using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExistePeriodoFechmantoPorUePeriodoEscolarQuery : IRequest<bool>
    {
        public ExistePeriodoFechmantoPorUePeriodoEscolarQuery(long ueId, long periodoEscolarId)
        {
            UeId = ueId;
            PeriodoEscolarId = periodoEscolarId;
        }

        public long UeId { get; set; }
        public long PeriodoEscolarId { get; set; }
    }

    public class ExistePeriodoFechmantoPorUePeriodoEscolarQueryValidator : AbstractValidator<ExistePeriodoFechmantoPorUePeriodoEscolarQuery>
    {
        public ExistePeriodoFechmantoPorUePeriodoEscolarQueryValidator()
        {
            RuleFor(c => c.UeId)
               .NotEmpty()
               .WithMessage("O id da UE deve ser informado para consulta de existencia de periodo de fechamento.");

            RuleFor(c => c.PeriodoEscolarId)
               .NotEmpty()
               .WithMessage("O id do periodo escolar deve ser informado para consulta de existencia de periodo de fechamento.");
        }
    }
}
