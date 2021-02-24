using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
   public class ObtemUsuarioCEFAIDaDreQueryValidator : AbstractValidator<ObtemUsuarioCEFAIDaDreQuery>
    {
        public ObtemUsuarioCEFAIDaDreQueryValidator()
        {
            RuleFor(c => c.CodigoDRE)
            .NotEmpty()
            .WithMessage("O código da DRE deve ser informado.");
        }
    }
}
