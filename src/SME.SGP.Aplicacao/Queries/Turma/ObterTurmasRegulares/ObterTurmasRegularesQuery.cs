using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasRegularesQuery : IRequest<IEnumerable<string>>
    {
        public ObterTurmasRegularesQuery(string[] codigosTurmas)
        {
            CodigosTurmas = codigosTurmas;
        }

        public string[] CodigosTurmas { get; set; }
    }

    public class ObterTurmasRegularesQueryValidator : AbstractValidator<ObterTurmasRegularesQuery>
    {
        public ObterTurmasRegularesQueryValidator()
        {
            RuleFor(c => c.CodigosTurmas)
                .NotNull()
                .WithMessage("O código das turmas deve ser informado para obter turmas regulares.");
        }
    }
}
