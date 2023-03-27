using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public Turma Turma { get; set; }
        public long[] ComponentesCurricularesId { get; set; }
        public IEnumerable<long> PeriodosEscolaresIds { get; set; }

        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(Turma turma, long[] componentesCurricularesId)
        {
            Turma = turma;
            ComponentesCurricularesId = componentesCurricularesId;
        }

        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(Turma turma, long[] componentesCurricularesId, IEnumerable<long> periodosEscolaresIds)
            :this(turma, componentesCurricularesId)
        {
            PeriodosEscolaresIds = periodosEscolaresIds;
        }

        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(Turma turma, long[] componentesCurricularesId, long periodoEscolarId)
            : this(turma, componentesCurricularesId)
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

            RuleFor(x => x.ComponentesCurricularesId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado para consulta das frequências dos alunos.");

            RuleForEach(x => x.PeriodosEscolaresIds)
                .NotEmpty()
                .WithMessage("O período escolar deve ser informado para consulta das frequências dos alunos.");
        }
    }
}