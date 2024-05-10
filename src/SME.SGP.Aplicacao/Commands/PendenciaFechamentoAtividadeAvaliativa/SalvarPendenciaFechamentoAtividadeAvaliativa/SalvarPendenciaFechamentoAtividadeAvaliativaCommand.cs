using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaFechamentoAtividadeAvaliativaCommand : IRequest<bool>
    {
        public SalvarPendenciaFechamentoAtividadeAvaliativaCommand(long atividadeAvaliativaId, long pendenciaFechamentoId)
        {
            AtividadeAvaliativaId = atividadeAvaliativaId;
            PendenciaFechamentoId = pendenciaFechamentoId;
        }

        public long AtividadeAvaliativaId { get; set; }
        public long PendenciaFechamentoId { get; set; }
    }

    public class SalvarPendenciaFechamentoAtividadeAvaliativaCommandValidator : AbstractValidator<SalvarPendenciaFechamentoAtividadeAvaliativaCommand>
    {
        public SalvarPendenciaFechamentoAtividadeAvaliativaCommandValidator()
        {
            RuleFor(c => c.AtividadeAvaliativaId)
               .NotEmpty()
               .WithMessage("O id da atividade avaliativa deve ser informada.");
            RuleFor(c => c.PendenciaFechamentoId)
              .NotEmpty()
              .WithMessage("O id da pendência do fechamento deve ser informado.");
        }
    }
}
