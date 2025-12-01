using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaComUeEDrePorIdQuery : IRequest<Turma>
    {
        public ObterTurmaComUeEDrePorIdQuery(long turmaId)
        {
            TurmaId = turmaId;
        }

        public long TurmaId { get; set; }
    }

    public class ObterTurmaComUeEDrePorIdQueryValidator : AbstractValidator<ObterTurmaComUeEDrePorIdQuery>
    {
        public ObterTurmaComUeEDrePorIdQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .Must(a => a > 0)
                .WithMessage("O id da turma deve ser informado para consulta!");
        }
    }
}
