using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterRestruturacoesPlanoAEEQueryValidator : AbstractValidator<ObterRestruturacoesPlanoAEEQuery>
    {
        public ObterRestruturacoesPlanoAEEQueryValidator()
        {
            RuleFor(c => c.PlanoId)
            .NotEmpty()
            .WithMessage("O ID do Plano deve ser informado!");
        }
    }
}
