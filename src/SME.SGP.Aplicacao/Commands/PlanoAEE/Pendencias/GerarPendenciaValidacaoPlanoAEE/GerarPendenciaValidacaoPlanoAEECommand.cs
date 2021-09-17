using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaValidacaoPlanoAEECommand : IRequest<bool>
    {
        public long PlanoAEEId { get; set; }

        public GerarPendenciaValidacaoPlanoAEECommand(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }
    }

    public class GerarPendenciaValidacaoPlanoAEECommandValidator : AbstractValidator<GerarPendenciaValidacaoPlanoAEECommand>
    {
        public GerarPendenciaValidacaoPlanoAEECommandValidator()
        {

            RuleFor(c => c.PlanoAEEId)
               .NotEmpty()
               .WithMessage("O id do plano AEE precisa ser informado.");
        }
    }
}
