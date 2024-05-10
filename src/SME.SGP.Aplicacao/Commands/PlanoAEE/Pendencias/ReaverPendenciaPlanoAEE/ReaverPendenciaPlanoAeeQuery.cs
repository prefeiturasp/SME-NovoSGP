using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ReaverPendenciaPlanoAeeQuery : IRequest<bool>
    {
        public ReaverPendenciaPlanoAeeQuery(long planoAeeId)
        {
            PlanoAeeId = planoAeeId;
        }
        public long PlanoAeeId { get; set; }
    }

    public class ReaverPendenciaPlanoAeeQueryValidator : AbstractValidator<ReaverPendenciaPlanoAeeQuery>
    {
        public ReaverPendenciaPlanoAeeQueryValidator()
        {
            RuleFor(x => x.PlanoAeeId)
                .GreaterThan(0)
                .WithMessage("Informe o id do plano AEE.");
        }
    }
}
