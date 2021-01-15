using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessorDaTurmaPorAulaIdQuery : IRequest<ProfessorDto>
    {
        public ObterProfessorDaTurmaPorAulaIdQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }
    public class ObterProfessorDaTurmaPorAulaIdQueryValidator : AbstractValidator<ObterProfessorDaTurmaPorAulaIdQuery>
    {
        public ObterProfessorDaTurmaPorAulaIdQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O Id da aula deve ser informado.");
        }
    }
}
