using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class PendenciaFechamentoAulaCommand : IRequest<bool>
    {
        public PendenciaFechamentoAulaCommand(long aulaId, long pendenciaFechamentoId)
        {
            AulaId = aulaId;
            PendenciaFechamentoId = pendenciaFechamentoId;
        }

        public long AulaId { get; set; }
        public long PendenciaFechamentoId { get; set; }
    }

    public class PendenciaFechamentoAulaCommandValidator : AbstractValidator<PendenciaFechamentoAulaCommand>
    {
        public PendenciaFechamentoAulaCommandValidator()
        {
            RuleFor(c => c.AulaId)
               .NotEmpty()
               .WithMessage("O id da aula deve ser informada.");
            RuleFor(c => c.PendenciaFechamentoId)
              .NotEmpty()
              .WithMessage("O id da pendência do fechamento deve ser informado.");
        }
    }
}
