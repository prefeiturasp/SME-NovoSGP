using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoDREPorTurmaIdQuery : IRequest<string>
    {
        public ObterCodigoDREPorTurmaIdQuery(long turmaId)
        {
            TurmaId = turmaId;
        }

        public long TurmaId { get; }
    }

    public class ObterCodigoDREPorTurmaIdQueryValidator : AbstractValidator<ObterCodigoDREPorTurmaIdQuery>
    {
        public ObterCodigoDREPorTurmaIdQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para consulta de sua DRE.");
        }
    }
}
