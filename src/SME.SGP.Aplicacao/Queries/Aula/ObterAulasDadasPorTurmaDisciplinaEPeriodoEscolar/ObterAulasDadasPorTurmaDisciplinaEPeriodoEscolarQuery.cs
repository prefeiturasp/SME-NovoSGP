using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery : IRequest<int>
    {
        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public IEnumerable<long> PeriodosEscolaresIds { get; set; }
        public long TipoCalendarioId { get; set; }

        private ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(string turmaCodigo, long componenteCurricularId, long tipoCalendarioId)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularId = componenteCurricularId;
            TipoCalendarioId = tipoCalendarioId;
        }

        public ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(string turmaCodigo, long componenteCurricularId, long tipoCalendarioId, long periodoEscolarId)
            :this(turmaCodigo, componenteCurricularId, tipoCalendarioId)
        {
            PeriodosEscolaresIds = new List<long> { periodoEscolarId };
        }

        public ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(string turmaCodigo, long componenteCurricularId, long tipoCalendarioId, IEnumerable<long> periodosEscolaresIds)
            : this(turmaCodigo, componenteCurricularId, tipoCalendarioId)
        {
            PeriodosEscolaresIds = periodosEscolaresIds;
        }
    }

    public class ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQueryValidator : AbstractValidator<ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery>
    {
        public ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQueryValidator()
        {
            RuleFor(x => x.TurmaCodigo)
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