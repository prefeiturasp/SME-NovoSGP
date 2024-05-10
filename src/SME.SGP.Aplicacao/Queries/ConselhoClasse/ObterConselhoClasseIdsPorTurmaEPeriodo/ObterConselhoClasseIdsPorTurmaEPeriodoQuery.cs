using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseIdsPorTurmaEPeriodoQuery : IRequest<long[]>
    {
        public ObterConselhoClasseIdsPorTurmaEPeriodoQuery(string[] turmasCodigos, long? periodoEscolarId)
        {
            TurmasCodigos = turmasCodigos;
            PeriodoEscolarId = periodoEscolarId;
        }

        public string[] TurmasCodigos { get; set; }
        public long? PeriodoEscolarId { get; set; }
    }

    public class ObterConselhoClasseIdsPorTurmaEPeriodoQueryValidator : AbstractValidator<ObterConselhoClasseIdsPorTurmaEPeriodoQuery>
    {
        public ObterConselhoClasseIdsPorTurmaEPeriodoQueryValidator()
        {
            RuleFor(c => c.TurmasCodigos)
               .NotEmpty()
               .WithMessage("Os códigos de turma devem ser informados");
        }
    }
}
