using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaEncaminhamentoAEEPorIdQueryValidator : AbstractValidator<ObterPendenciaEncaminhamentoAEEPorIdQuery>
    {
        public ObterPendenciaEncaminhamentoAEEPorIdQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoAEEId)
               .NotEmpty()
               .WithMessage("O Id encaminhamento deve ser informado.");
        }
    }
}
