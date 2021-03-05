using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDadasPorTurmaIdEPeriodoEscolarQuery : IRequest<int>
    {
        public long TurmaId { get; set; }
        public IEnumerable<long> PeriodosEscolaresIds { get; set; }
        public long TipoCalendarioId { get; set; }

        public ObterAulasDadasPorTurmaIdEPeriodoEscolarQuery(long turmaId, List<long> periodosEscolaresIds, long tipoCalendarioId)
        {
            TurmaId = turmaId;
            TipoCalendarioId = tipoCalendarioId;
            PeriodosEscolaresIds = periodosEscolaresIds;
        }
    }

    public class ObterAulasDadasPorTurmaIdEPeriodoEscolarQueryValidator : AbstractValidator<ObterAulasDadasPorTurmaIdEPeriodoEscolarQuery>
    {
        public ObterAulasDadasPorTurmaIdEPeriodoEscolarQueryValidator()
        {
            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para a consulta de aulas dadas.");

            RuleForEach(x => x.PeriodosEscolaresIds)
                .NotEmpty()
                .WithMessage("O período escolar deve ser informado para a consulta de aulas dadas.");

            RuleFor(x => x.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado para a consulta de aulas dadas.");
        }
    }
}