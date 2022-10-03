using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorDisciplinaETurmaQuery : IRequest<int>
    {
        public ObterTotalAulasPorDisciplinaETurmaQuery(DateTime dataAula, string disciplinaId, params string[] turmasId)
        {
            DataAula = dataAula;
            DisciplinaId = disciplinaId;
            TurmasId = turmasId;
        }

        public DateTime DataAula { get; }
        public string DisciplinaId { get; }
        public string[] TurmasId { get; }
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
