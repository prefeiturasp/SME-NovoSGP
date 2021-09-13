using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasProgramaQuery : IRequest<IEnumerable<string>>
    {
        public ObterTurmasProgramaQuery(string[] codigosTurmas)
        {
            CodigosTurmas = codigosTurmas;
        }

        public string[] CodigosTurmas { get; set; }
    }

    public class ObterTurmasProgramaQueryValidator : AbstractValidator<ObterTurmasProgramaQuery>
    {
        public ObterTurmasProgramaQueryValidator()
        {
            RuleFor(c => c.CodigosTurmas)
            .NotEmpty()
            .WithMessage("CodigosTurmas deve ser informado.");
        }
    }

}
