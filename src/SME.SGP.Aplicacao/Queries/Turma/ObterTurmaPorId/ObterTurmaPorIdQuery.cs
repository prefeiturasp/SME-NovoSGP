using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorIdQuery : IRequest<Turma>
    {
        public ObterTurmaPorIdQuery() { }
        public ObterTurmaPorIdQuery(long turmaId)
        {
            TurmaId = turmaId;
        }

        public long TurmaId { get; set; }
    }

    public class ObterTurmaPorIdQueryValidator : AbstractValidator<ObterTurmaPorIdQuery>
    {

        public ObterTurmaPorIdQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado.");
        }
    }
}
