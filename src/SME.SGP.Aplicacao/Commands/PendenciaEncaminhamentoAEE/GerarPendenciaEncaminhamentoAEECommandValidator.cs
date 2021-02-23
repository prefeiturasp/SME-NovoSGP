using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaEncaminhamentoAEECommandValidator : AbstractValidator<GerarPendenciaEncaminhamentoAEECommand>
    {
        public GerarPendenciaEncaminhamentoAEECommandValidator()
        {
            RuleFor(c => c.TipoPendencia)
               .NotEmpty()
               .WithMessage("O tipo de pendência deve ser informado para geração da pendência de Encaminhamento AEE.");
        }
    }
}
