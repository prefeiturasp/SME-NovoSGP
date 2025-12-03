using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirInatividadesAtendimentoNotificacaoPorIdNAAPACommand : IRequest<bool>
    {
        public ExcluirInatividadesAtendimentoNotificacaoPorIdNAAPACommand(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }
        public long EncaminhamentoNAAPAId { get; }
    }

    public class ExcluirInatividadesAtendimentoNotificacaoPorIdNAAPACommandValidator : AbstractValidator<ExcluirInatividadesAtendimentoNotificacaoPorIdNAAPACommand>
    {
        public ExcluirInatividadesAtendimentoNotificacaoPorIdNAAPACommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
                .GreaterThan(0)
                .WithMessage("O Id do encaminhamento NAAPA deve ser informado para exclusão do vínculo com a notificação.");
        }
    }
}
