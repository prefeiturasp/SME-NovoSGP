using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public Turma Turma { get; set; }
        public long[] ComponentesCurricularesId { get; set; }
        public IEnumerable<long> PeriodosEscolaresIds { get; set; }
        public string Professor { get; set; }

        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(Turma turma, long[] componentesCurricularesId, string professor = null)
        {
            Turma = turma;
            ComponentesCurricularesId = componentesCurricularesId;
            Professor = professor;
        }

        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(Turma turma, long[] componentesCurricularesId, IEnumerable<long> periodosEscolaresIds, string professor = null)
            : this(turma, componentesCurricularesId)
        {
            PeriodosEscolaresIds = periodosEscolaresIds;
            Professor = professor;
        }

        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(Turma turma, long[] componentesCurricularesId, long periodoEscolarId, string professor = null)
            : this(turma, componentesCurricularesId)
        {
            PeriodosEscolaresIds = new List<long> { periodoEscolarId };
            Professor = professor;
        }
    }

    public class ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQueryValidator : AbstractValidator<ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery>
    {
        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQueryValidator()
        {
            RuleFor(x => x.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para consulta das frequências dos alunos.");

            RuleFor(x => x.ComponentesCurricularesId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado para consulta das frequências dos alunos.");

            RuleForEach(x => x.PeriodosEscolaresIds)
                .NotEmpty()
                .WithMessage("O período escolar deve ser informado para consulta das frequências dos alunos.");
        }
    }
}