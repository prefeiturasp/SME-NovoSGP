using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoDevolutivasPorTurmaIdQuery : IRequest<ConsolidacaoDevolutivas>
    {
        public ObterConsolidacaoDevolutivasPorTurmaIdQuery(long turmaId)
        {
            TurmaId = turmaId;
        }

        public long TurmaId { get; set; }
    }

    public class ObterConsolidacaoDevolutivasPorTurmaIdQueryValidator : AbstractValidator<ObterConsolidacaoDevolutivasPorTurmaIdQuery>
    {
        public ObterConsolidacaoDevolutivasPorTurmaIdQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da turma para obter a consolidação devolutivas.");
        }
    }
}
