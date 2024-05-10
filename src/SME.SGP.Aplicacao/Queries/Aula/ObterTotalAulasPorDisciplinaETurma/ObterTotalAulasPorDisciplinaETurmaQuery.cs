using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorDisciplinaETurmaQuery : IRequest<int>
    {
        public ObterTotalAulasPorDisciplinaETurmaQuery(DateTime dataAula, string[] disciplinaIdsConsideradas, DateTime? dataMatriculaAluno = null, DateTime? dataSituacaoAluno = null, string professor = null, params string[] turmasId)
        {
            DataAula = dataAula;
            DisciplinaIdsConsideradas = disciplinaIdsConsideradas;
            TurmasId = turmasId;
            DataMatriculaAluno = dataMatriculaAluno;
            DataSituacaoAluno = dataSituacaoAluno;
            Professor = professor;
        }

        public DateTime DataAula { get; }
        public string[] DisciplinaIdsConsideradas { get; }
        public string[] TurmasId { get; }
        public DateTime? DataMatriculaAluno { get; set; }
        public DateTime? DataSituacaoAluno { get; set; }
        public string Professor { get; set; }
    }

    public class ObterTotalAulasPorDisciplinaETurmaQueryValidator : AbstractValidator<ObterTotalAulasPorDisciplinaETurmaQuery>
    {
        public ObterTotalAulasPorDisciplinaETurmaQueryValidator()
        {
            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("Data de referência deve ser informada para consulta do total de aulas no Bimestre");

            RuleFor(a => a.TurmasId)
                .NotEmpty()
                .WithMessage("Turma deve ser informada para consulta do total de aulas no Bimestre");
        }
    }
}
