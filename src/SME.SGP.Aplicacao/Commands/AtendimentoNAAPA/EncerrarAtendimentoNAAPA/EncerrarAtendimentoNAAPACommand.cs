using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class EncerrarAtendimentoNAAPACommand : IRequest<bool>
    {
        public EncerrarAtendimentoNAAPACommand(long encaminhamentoId, string motivoEncerramento)
        {
            EncaminhamentoId = encaminhamentoId;
            MotivoEncerramento = motivoEncerramento;
        }

        public long EncaminhamentoId { get; set; }
        public string MotivoEncerramento { get; set; }
    }
    public class EncerrarAtendimentoNAAPACommandValidator : AbstractValidator<EncerrarAtendimentoNAAPACommand>
    {
        public EncerrarAtendimentoNAAPACommandValidator()
        {
            RuleFor(x => x.EncaminhamentoId)
                   .GreaterThan(0)
                    .WithMessage("O Id do Atendimento deve ser informado!");
            RuleFor(x => x.MotivoEncerramento)
                   .GreaterThan(string.Empty)
                   .WithMessage("O Motivo do Encerramento deve ser informado!");
        }
    }
}
