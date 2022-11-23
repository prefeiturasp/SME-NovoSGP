using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterSomenteTurmaPorIdQuery : IRequest<Turma>
    {
        public ObterSomenteTurmaPorIdQuery() { }
        public ObterSomenteTurmaPorIdQuery(long turmaId)
        {
            TurmaId = turmaId;
        }

        public long TurmaId { get; set; }
    }

    public class ObterSomenteTurmaPorIdQueryValidator : AbstractValidator<ObterSomenteTurmaPorIdQuery>
    {

        public ObterSomenteTurmaPorIdQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado.");
        }
    }
}
