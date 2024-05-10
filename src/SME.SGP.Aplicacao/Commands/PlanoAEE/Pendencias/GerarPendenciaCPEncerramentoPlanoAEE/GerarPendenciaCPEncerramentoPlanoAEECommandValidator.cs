using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaCPEncerramentoPlanoAEECommandValidator : AbstractValidator<GerarPendenciaCPEncerramentoPlanoAEECommand>
    {
        public GerarPendenciaCPEncerramentoPlanoAEECommandValidator()
        {
            RuleFor(c => c.PlanoAEEId)
                   .NotEmpty()
                   .WithMessage("O plano AEE ID precisa ser informado.");
        }
    }
}
