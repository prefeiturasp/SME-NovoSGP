using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
   public class ObterPAEETurmaQueryValidator : AbstractValidator<ObterPAEETurmaQuery>
    {
        public ObterPAEETurmaQueryValidator()
        {
            RuleFor(c => c.CodigoDRE)
            .NotEmpty()
            .WithMessage("O código da DRE deve ser informado.");

            RuleFor(c => c.CodigoUE)
            .NotEmpty()
            .WithMessage("O código da UE deve ser informado.");
        }
    }
}
