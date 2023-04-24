using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigoPorIdQuery : IRequest<string>
    {
        public long TurmaId { get; set; }

        public ObterTurmaCodigoPorIdQuery(long turmaId)
        {
            TurmaId = turmaId;
        }
    }
    public class ObterTurmaCodigoPorIdQueryValidator : AbstractValidator<ObterTurmaCodigoPorIdQuery>
    {

        public ObterTurmaCodigoPorIdQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O Id da turma deve ser informado.");
        }
    }
}
