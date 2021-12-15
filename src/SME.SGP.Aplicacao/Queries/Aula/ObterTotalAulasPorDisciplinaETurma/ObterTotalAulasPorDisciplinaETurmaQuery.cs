using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorDisciplinaETurmaQuery : IRequest<int>
    {
        public ObterTotalAulasPorDisciplinaETurmaQuery(DateTime dataAula, string disciplinaId, string turmaId)
        {
            DataAula = dataAula;
            DisciplinaId = disciplinaId;
            TurmaId = turmaId;
        }

        public DateTime DataAula { get; }
        public string DisciplinaId { get; }
        public string TurmaId { get; }
    }

    public class ObterTotalAulasPorDisciplinaETurmaQueryValidator : AbstractValidator<ObterTotalAulasPorDisciplinaETurmaQuery>
    {
        public ObterTotalAulasPorDisciplinaETurmaQueryValidator()
        {
            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("Data de referência deve ser informada para consulta do total de aulas no Bimestre");

            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("Turma deve ser informada para consulta do total de aulas no Bimestre");
        }
    }
}
