using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoRegistroItineranciaRecusadoCommandValidator : AbstractValidator<NotificacaoRegistroItineranciaRecusadoCommand>
    {
        public NotificacaoRegistroItineranciaRecusadoCommandValidator()
        {
            RuleFor(c => c.ItineranciaId)
               .NotEmpty()
               .WithMessage("O ID da Itinerância deve ser informado");
            RuleFor(c => c.WorkflowId)
               .NotEmpty()
               .WithMessage("O ID do Workflow deve ser informado");
        }
    }
}
