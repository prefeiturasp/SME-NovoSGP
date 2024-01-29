using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery : IRequest<int>
    {
        public string TurmaCodigo { get; set; }
        public long[] ComponentesCurricularesId { get; set; }
        public IEnumerable<long> PeriodosEscolaresIds { get; set; }
        public long TipoCalendarioId { get; set; }
        public string Professor { get; set; }

        private ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(string turmaCodigo, long[] componentesCurricularesId, long tipoCalendarioId)
        {
            TurmaCodigo = turmaCodigo;
            ComponentesCurricularesId = componentesCurricularesId;
            TipoCalendarioId = tipoCalendarioId;
        }

        public ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(string turmaCodigo, long[] componentesCurricularesId, long tipoCalendarioId, long periodoEscolarId, string professor = null)
            :this(turmaCodigo, componentesCurricularesId, tipoCalendarioId)
        {
            PeriodosEscolaresIds = new List<long> { periodoEscolarId };
            Professor = professor;
        }

        public ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(string turmaCodigo, long[] componentesCurricularesId, long tipoCalendarioId, IEnumerable<long> periodosEscolaresIds, string professor = null)
            : this(turmaCodigo, componentesCurricularesId, tipoCalendarioId)
        {
            PeriodosEscolaresIds = periodosEscolaresIds;
            Professor = professor;
        }
    }

    public class ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQueryValidator : AbstractValidator<ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery>
    {
        public ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQueryValidator()
        {
            RuleFor(x => x.TurmaCodigo)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para a consulta de aulas dadas.");

            RuleFor(x => x.ComponentesCurricularesId)
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