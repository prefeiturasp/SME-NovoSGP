using FluentValidation;
using MediatR;
using System.Collections.Generic;

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
