using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommandValidator : AbstractValidator<GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand>
    {
        public GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommandValidator()
        {
            RuleFor(c => c.EncaminhamentoAEE)
                   .NotEmpty()
                   .WithMessage("O encaminhamento precisa ser informado.");
        }
    }
}
