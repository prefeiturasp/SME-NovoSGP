using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class RegistrarFrequenciaTurmaEvasaoCommandValidator : AbstractValidator<RegistrarFrequenciaTurmaEvasaoCommand>
    {
        public RegistrarFrequenciaTurmaEvasaoCommandValidator()
        {
            RuleFor(c => c.TurmaId)
                .GreaterThan(0)
                .WithMessage("O id da turma deve ser informado.");

            RuleFor(c => c.Mes)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(12)
                .WithMessage("Um mês válido deve ser informado.");
        }
    }
}
