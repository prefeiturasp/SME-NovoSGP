using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AlterarDataUltimaNotificacaoInatividadeAtendimentoNAAPACommand : IRequest<bool>
    {
        public AlterarDataUltimaNotificacaoInatividadeAtendimentoNAAPACommand(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }
        public long EncaminhamentoId { get; set; }
    }

    public class AlterarDataUltimaNotificacaoInatividadeAtendimentoNAAPACommandValidator : AbstractValidator<AlterarDataUltimaNotificacaoInatividadeAtendimentoNAAPACommand>
    {
        public AlterarDataUltimaNotificacaoInatividadeAtendimentoNAAPACommandValidator()
        {
            RuleFor(a => a.EncaminhamentoId)
                .NotEmpty()
                .WithMessage("É id do encaminhamento naapa deve ser informado para atualização da última data de notificação de inatividade atendimento.");
        }
    }
}
