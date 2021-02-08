using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAeeCommandValidator : AbstractValidator<SalvarPlanoAeeCommand>
    {
        public SalvarPlanoAeeCommandValidator()
        {
            RuleFor(x => x.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("A turma deve ser informada!");
        }
    }
}
