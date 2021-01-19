using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery : IRequest<int>
    {
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public IEnumerable<long> PeriodosEscolaresIds { get; set; }
        public long TipoCalendarioId { get; set; }

        private ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(long turmaId, long componenteCurricularId, long tipoCalendarioId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            TipoCalendarioId = tipoCalendarioId;
        }

        public ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(long turmaId, long componenteCurricularId, long tipoCalendarioId, long periodoEscolarId)
            :this(turmaId, componenteCurricularId, tipoCalendarioId)
        {
            PeriodosEscolaresIds = new List<long> { periodoEscolarId };
        }

        public ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(long turmaId, long componenteCurricularId, long tipoCalendarioId, IEnumerable<long> periodosEscolaresIds)
            : this(turmaId, componenteCurricularId, tipoCalendarioId)
        {
            PeriodosEscolaresIds = periodosEscolaresIds;
        }
    }

    public class ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQueryValidator : AbstractValidator<ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery>
    {
        public ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQueryValidator()
        {
            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para a consulta de aulas dadas.");

            RuleFor(x => x.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado para a consulta de aulas dadas.");

            RuleForEach(x => x.PeriodosEscolaresIds)
                .NotEmpty()
                .WithMessage("O período escolar deve ser informado para a consulta de aulas dadas.");

            RuleFor(x => x.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado para a consulta de aulas dadas.");
        }
    }
}