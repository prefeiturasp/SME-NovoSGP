using FluentValidation;

namespace SME.SGP.Aplicacao.Commands.Frequencia.ConciliacaoFrequenciaTurmaUe
{
    public class ConciliacaoFrequenciaTurmaUeCommandValidator : AbstractValidator<ConciliacaoFrequenciaTurmaUeCommand>
    {
        public ConciliacaoFrequenciaTurmaUeCommandValidator()
        {
            RuleFor(c => c.UeCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da UE deve ser informado.");

            RuleFor(a => a.AnoLetivo)
                .GreaterThanOrEqualTo(2020)
                .WithMessage("Um ano a partir de 2020 deve ser informado.");
        }
    }
}
