using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClassePorTurmaEPeriodoQuery : IRequest<ConselhoClasse>
    {
        public long TurmaId { get; set; }
        public long? PeriodoEscolarId { get; set; }

        public ObterConselhoClassePorTurmaEPeriodoQuery(long turmaId, long? periodoEscolarId)
        {
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
        }

        public class ObterPorTurmaEPeriodoQueryValidator : AbstractValidator<ObterConselhoClassePorTurmaEPeriodoQuery>
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
