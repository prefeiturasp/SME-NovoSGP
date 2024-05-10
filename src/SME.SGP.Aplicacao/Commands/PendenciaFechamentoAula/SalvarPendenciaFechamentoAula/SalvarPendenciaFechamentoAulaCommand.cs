using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaFechamentoAulaCommand : IRequest<bool>
    {
        public SalvarPendenciaFechamentoAulaCommand(long aulaId, long pendenciaFechamentoId)
        {
            AulaId = aulaId;
            PendenciaFechamentoId = pendenciaFechamentoId;
        }

        public long AulaId { get; set; }
        public long PendenciaFechamentoId { get; set; }
    }

    public class PendenciaFechamentoAulaCommandValidator : AbstractValidator<SalvarPendenciaFechamentoAulaCommand>
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
