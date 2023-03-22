using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarProfessorQuery : IRequest<IEnumerable<FrequenciaAlunoDto>>
    {
        public Turma Turma { get; set; }
        public long[] ComponentesCurricularesId { get; set; }
        public IEnumerable<long> PeriodosEscolaresIds { get; set; }
        public string RfProfessorTerritorioSaber { get; set; }

        public ObterRegistroFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarProfessorQuery(Turma turma, long[] componentesCurricularesId, string rfProfessorTerritorioSaber = null)
        {
            Turma = turma;
            ComponentesCurricularesId = componentesCurricularesId;
            RfProfessorTerritorioSaber = rfProfessorTerritorioSaber;
        }

        public ObterRegistroFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarProfessorQuery(Turma turma, long[] componentesCurricularesId, IEnumerable<long> periodosEscolaresIds, string rfProfessorTerritorioSaber = null)
            :this(turma, componentesCurricularesId)
        {
            PeriodosEscolaresIds = periodosEscolaresIds;
            RfProfessorTerritorioSaber = rfProfessorTerritorioSaber;
        }

        public ObterRegistroFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarProfessorQuery(Turma turma, long[] componentesCurricularesId, long periodoEscolarId, string rfProfessorTerritorioSaber = null)
            : this(turma, componentesCurricularesId)
        {
            PeriodosEscolaresIds = new List<long> { periodoEscolarId };
            RfProfessorTerritorioSaber = rfProfessorTerritorioSaber;
        }
    }

    public class ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQueryValidator : AbstractValidator<ObterRegistroFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarProfessorQuery>
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