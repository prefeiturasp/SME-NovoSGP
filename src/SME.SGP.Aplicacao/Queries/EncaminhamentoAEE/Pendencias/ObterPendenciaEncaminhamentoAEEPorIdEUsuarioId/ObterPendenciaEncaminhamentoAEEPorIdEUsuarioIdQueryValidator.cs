using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQueryValidator : AbstractValidator<ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery>
    {
        public ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoAEEId)
               .NotEmpty()
               .WithMessage("O Id encaminhamento deve ser informado.");

            RuleFor(c => c.UsuarioId)
               .NotEmpty()
               .WithMessage("O Id do usuário deve ser informado.");
        }
    }
}
