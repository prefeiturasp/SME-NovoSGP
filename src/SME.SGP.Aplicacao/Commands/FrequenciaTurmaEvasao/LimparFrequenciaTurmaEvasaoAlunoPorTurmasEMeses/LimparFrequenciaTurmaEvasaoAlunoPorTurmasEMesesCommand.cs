using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand : IRequest<bool>
    {
        public LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand(long[] turmasIds, int[] meses)
        {
            TurmasIds = turmasIds;
            Meses = meses;
        }

        public long[] TurmasIds { get; set; }
        public int[] Meses { get; set; }
    }

    public class LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommandValidator : AbstractValidator<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand>
    {
        public LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommandValidator()
        {
            RuleFor(c => c.TurmasIds)
                .NotEmpty()
                .WithMessage("Os códigos das turmas devem ser informados para limpar as informações de evasão.");

            RuleFor(c => c.Meses)
                .NotEmpty()
                .WithMessage("Os meses devem ser informados para limpar as informações de evasão.");
        }
    }
}
