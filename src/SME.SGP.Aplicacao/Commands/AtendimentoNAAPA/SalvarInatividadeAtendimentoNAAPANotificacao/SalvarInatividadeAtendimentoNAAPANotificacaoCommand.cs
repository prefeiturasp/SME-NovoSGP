using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarInatividadeAtendimentoNAAPANotificacaoCommand : IRequest<long>
    {
        public SalvarInatividadeAtendimentoNAAPANotificacaoCommand(long encaminhamentoNAAPAId, long notificacaoId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
            NotificacaoId = notificacaoId;
        }

        public long EncaminhamentoNAAPAId { get; set; }
        public long NotificacaoId { get; set; }
    }

    public class SalvarInatividadeAtendimentoNAAPANotificacaoCommandValidator : AbstractValidator<SalvarInatividadeAtendimentoNAAPANotificacaoCommand>
    {
        public SalvarInatividadeAtendimentoNAAPANotificacaoCommandValidator()
        {
            RuleFor(a => a.EncaminhamentoNAAPAId)
               .NotEmpty()
               .WithMessage("O id do atendimento NAAPA deve ser informado para o cadastro do vínculo com a notificação.");

            RuleFor(a => a.NotificacaoId)
               .NotEmpty()
               .WithMessage("O id da notificação deve ser informado para o cadastro do vínculo com a notificação.");
        }
    }
}
