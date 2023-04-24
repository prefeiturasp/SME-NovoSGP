using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class AtualizarnotificacaoMensagemPorIdsCommandValidator : AbstractValidator<AtualizarNotificacaoMensagemPorIdsCommand>
    {
        public AtualizarnotificacaoMensagemPorIdsCommandValidator()
        {
            RuleFor(c => c.Ids)
                .NotNull()
                .WithMessage("Os ids das notificações são obrigatórios para atualizar a mensagem das notificações");
            
            RuleFor(c => c.Mensagem)
                .NotEmpty()
                .WithMessage("A mensagem é obrigatório para atualizar as notificações");
        }
    }
}