using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class NotificarAprovacaoNotaConselhoCommand : IRequest
    {
        public NotificarAprovacaoNotaConselhoCommand(WFAprovacaoNotaConselho notasEmAprovacao,
                                                     long? codigoDaNotificacao,
                                                     string turmaCodigo,
                                                     long workflowId,
                                                     bool aprovada = true,
                                                     string justificativa = "",
                                                     double? notaAnterior = null,
                                                     long? conceitoAnterior = null)
        {
            NotasEmAprovacao = notasEmAprovacao;
            TurmaCodigo = turmaCodigo;
            CodigoDaNotificacao = codigoDaNotificacao;
            WorkFlowId = workflowId;
            Aprovada = aprovada;
            Justificativa = justificativa;
            NotaAnterior = notaAnterior;
            ConceitoAnterior = conceitoAnterior;
        }

        public WFAprovacaoNotaConselho NotasEmAprovacao { get; }
        public long? CodigoDaNotificacao { get; }
        public string TurmaCodigo { get; }
        public long WorkFlowId { get; }
        public bool Aprovada { get; }
        public string Justificativa { get; }
        public double? NotaAnterior { get; }
        public long? ConceitoAnterior { get; }
    }
    public class NotificarAprovacaoNotaConselhoCommandValidator : AbstractValidator<NotificarAprovacaoNotaConselhoCommand>
    {
        public NotificarAprovacaoNotaConselhoCommandValidator()
        {
            RuleFor(a => a.NotasEmAprovacao)
                .NotEmpty()
                .WithMessage("A aprovação deve conter notas");

            RuleFor(a => a.CodigoDaNotificacao)
                .NotEmpty()
                .WithMessage("O código da notificação ser informado para notificação de sua aprovação");

            RuleFor(a => a.WorkFlowId)
                .NotEmpty()
                .WithMessage("O código do workflow deve ser informado para notificação de sua aprovação");
        }
    }
}
