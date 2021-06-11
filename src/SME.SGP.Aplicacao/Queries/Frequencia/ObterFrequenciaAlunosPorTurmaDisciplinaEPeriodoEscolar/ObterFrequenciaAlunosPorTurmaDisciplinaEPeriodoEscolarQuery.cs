using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public Turma Turma { get; set; }
        public long ComponenteCurricularId { get; set; }
        public IEnumerable<long> PeriodosEscolaresIds { get; set; }

        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(Turma turma, long componenteCurricularId)
        {
            Turma = turma;
            ComponenteCurricularId = componenteCurricularId;
        }

        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(Turma turma, long componenteCurricularId, IEnumerable<long> periodosEscolaresIds)
            :this(turma, componenteCurricularId)
        {
            PeriodosEscolaresIds = periodosEscolaresIds;
        }

        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(Turma turma, long componenteCurricularId, long periodoEscolarId)
            : this(turma, componenteCurricularId)
        {
            PeriodosEscolaresIds = new List<long> { periodoEscolarId };
        }
    }

    public class ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQueryValidator : AbstractValidator<ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery>
    {
        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQueryValidator()
        {
            RuleFor(x => x.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para consulta das frequências dos alunos.");

            RuleFor(x => x.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado para consulta das frequências dos alunos.");

            RuleForEach(x => x.PeriodosEscolaresIds)
                .NotEmpty()
                .WithMessage("O período escolar deve ser informado para consulta das frequências dos alunos.");
        }
    }
}