using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAlunosNoSemestreQuery : IRequest<int>
    {
        public ObterTotalAlunosNoSemestreQuery(long turmaId, int anoLetivo, int semestre)
        {
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
            Semestre = semestre;
        }

        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public int Semestre { get; set; }
    }

    public class ObterTotalAlunosNoSemestreQueryValidator : AbstractValidator<ObterTotalAlunosNoSemestreQuery>
    {
        public ObterTotalAlunosNoSemestreQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para obter os dados de consolidação acompanhamento de aprendizagem");

            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para obter os dados de consolidação acompanhamento de aprendizagem");

            RuleFor(a => a.Semestre)
                .NotEmpty()
                .WithMessage("O semestre deve ser informado para obter os dados de consolidação acompanhamento de aprendizagem");
        }
    }
}
