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
        }
    }
}
