using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterUEPorTurmaIdQueryValidator : AbstractValidator<ObterUEPorTurmaIdQuery>
    {
        public ObterUEPorTurmaIdQueryValidator()
        {
            RuleFor(c => c.TurmaId)
            .NotEmpty()
            .WithMessage("A TurmaId deve ser informada para consulta das UEs.");

        }
    }
}
