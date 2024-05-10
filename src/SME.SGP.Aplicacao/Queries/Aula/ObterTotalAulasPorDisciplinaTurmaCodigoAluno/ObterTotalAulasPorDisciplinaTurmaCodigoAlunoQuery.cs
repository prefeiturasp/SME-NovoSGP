using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorDisciplinaTurmaCodigoAlunoQuery : IRequest<int>
    {
        public ObterTotalAulasPorDisciplinaTurmaCodigoAlunoQuery(DateTime dataAula, string codigoAluno, string disciplinaId, params string[] turmasId)
        {
            DataAula = dataAula;
            DisciplinaId = disciplinaId;
            TurmasId = turmasId;
            CodigoAluno = codigoAluno;
        }

        public DateTime DataAula { get; }
        public string CodigoAluno { get; }
        public string DisciplinaId { get; }
        public string[] TurmasId { get; }
        public DateTime? DataMatriculaAluno { get; set; }
        public DateTime? DataSituacaoAluno { get; set; }

    }

    public class ObterTotalAulasPorDisciplinaTurmaCodigoAlunoQueryValidator : AbstractValidator<ObterTotalAulasPorDisciplinaTurmaCodigoAlunoQuery>
    {
        public ObterTotalAulasPorDisciplinaTurmaCodigoAlunoQueryValidator()
        {
            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("Data de referência deve ser informada para consulta do total de aulas no Bimestre");

            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("Código do aluno deve ser informado para consulta do total de aulas no Bimestre");

            RuleFor(a => a.TurmasId)
                .NotEmpty()
                .WithMessage("Turma deve ser informada para consulta do total de aulas no Bimestre");
        }
    }
}
