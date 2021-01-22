using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAulaDiasNaoLetivosCommand : IRequest<bool>
    {
        public SalvarPendenciaAulaDiasNaoLetivosCommand(long aulaId, string motivo, long pendenciaId)
        {
            AulaId = aulaId;
            Motivo = motivo;
            PendenciaId = pendenciaId;
        }

        public long AulaId { get; set; }
        public string Motivo { get; set; }
        public long PendenciaId { get; set; }
    }

    public class SalvarPendenciaAulaDiasNaoLetivosCommandValidator : AbstractValidator<SalvarPendenciaAulaDiasNaoLetivosCommand>
    {
        public SalvarPendenciaAulaDiasNaoLetivosCommandValidator()
        {
            RuleFor(c => c.AulaId)
            .NotEmpty()
            .WithMessage("O Id da aula deve ser informada para geração de pendência.");

            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O Id da pendencia deve ser informada para geração de pendência.");
        }

    }
}
