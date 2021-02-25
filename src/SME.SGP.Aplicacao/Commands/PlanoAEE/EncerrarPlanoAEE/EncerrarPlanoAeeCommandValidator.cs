using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class EncerrarPlanoAeeCommandValidator : AbstractValidator<EncerrarPlanoAeeCommand>
    {
        public EncerrarPlanoAeeCommandValidator()
        {
            RuleFor(x => x.PlanoId)
                   .GreaterThan(0)
                   .WithMessage("O ID do Plano é obrigatório!");
        }
    }
}
