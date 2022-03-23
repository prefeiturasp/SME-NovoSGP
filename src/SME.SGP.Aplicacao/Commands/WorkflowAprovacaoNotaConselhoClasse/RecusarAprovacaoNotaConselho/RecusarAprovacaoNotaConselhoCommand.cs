using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RecusarAprovacaoNotaConselhoCommand : IRequest
    {
        public RecusarAprovacaoNotaConselhoCommand(WFAprovacaoNotaConselho notasEmAprovacao,
                                                   long? codigoDaNotificacao,
                                                   string turmaCodigo,
                                                   long workflowId,
                                                   string justificativa,
                                                   double? notaAnterior = null,
                                                   long? conceitoAnterior = null)
        {
            NotasEmAprovacao = notasEmAprovacao;
            CodigoDaNotificacao = codigoDaNotificacao;
            TurmaCodigo = turmaCodigo;
            WorkflowId = workflowId;
            Justificativa = justificativa;
            NotaAnterior = notaAnterior;
            ConceitoAnterior = conceitoAnterior;
        }

        public WFAprovacaoNotaConselho NotasEmAprovacao { get; }
        public long? CodigoDaNotificacao { get; }
        public string TurmaCodigo { get; }
        public long WorkflowId { get; }
        public string Justificativa { get; }
        public double? NotaAnterior { get; }
        public long? ConceitoAnterior { get; }
    }

    public class RecusarAprovacaoNotaConselhoCommandValidator : AbstractValidator<RecusarAprovacaoNotaConselhoCommand>
    {
        public RecusarAprovacaoNotaConselhoCommandValidator()
        {
            RuleFor(a => a.NotasEmAprovacao)
                .NotEmpty()
                .WithMessage("A aprovação deve conter notas");

            RuleFor(a => a.CodigoDaNotificacao)
                .NotEmpty()
                .WithMessage("O código da notificação ser informado para notificação de recusa");

            RuleFor(a => a.WorkflowId)
                .NotEmpty()
                .WithMessage("O código do workflow deve ser informado para notificação de recusa");

            RuleFor(a => a.Justificativa)
                .NotEmpty()
                .WithMessage("A justificativa da recusa deve ser informada para notificação");
        }
    }
}
