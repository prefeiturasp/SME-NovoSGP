using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommandValidator : AbstractValidator<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand>
    {
        public LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommandValidator()
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
