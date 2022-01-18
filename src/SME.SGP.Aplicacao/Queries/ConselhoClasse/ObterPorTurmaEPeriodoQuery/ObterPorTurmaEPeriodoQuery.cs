using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPorTurmaEPeriodoQuery : IRequest<ConselhoClasse>
    {
        public long TurmaId { get; set; }
        public long? PeriodoEscolarId { get; set; }

        public ObterPorTurmaEPeriodoQuery(long turmaId, long? periodoEscolarId)
        {
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
        }

        public class ObterPorTurmaEPeriodoQueryValidator : AbstractValidator<ObterPorTurmaEPeriodoQuery>
        {
            public ObterPorTurmaEPeriodoQueryValidator()
            {
                RuleFor(c => c.TurmaId)
                    .NotEmpty()
                    .WithMessage("O id da turma deve ser informado.");
            }
        }
    }
}
