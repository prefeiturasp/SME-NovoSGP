using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaIdsPorCodigosQuery : IRequest<IEnumerable<long>>
    {
        public string[] TurmasCodigo { get; }

        public ObterTurmaIdsPorCodigosQuery(string[] turmasCodigo)
        {
            TurmasCodigo = turmasCodigo;
        }
    }
    public class ObterTurmaIdsPorCodigosValidator : AbstractValidator<ObterTurmaIdsPorCodigosQuery>
    {

        public ObterTurmaIdsPorCodigosValidator()
        {
            RuleFor(c => c.TurmasCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }
}
