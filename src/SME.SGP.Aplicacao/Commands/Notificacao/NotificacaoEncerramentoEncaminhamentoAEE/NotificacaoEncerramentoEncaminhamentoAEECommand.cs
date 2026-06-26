using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoEncerramentoEncaminhamentoAEECommand : IRequest<bool>
    {
        public long EncaminhamentoAEEId { get; set; }
        public string UsuarioRF { get; set; }
        public string UsuarioNome { get; set; }


        public NotificacaoEncerramentoEncaminhamentoAEECommand(long encaminhamentoAEEId, string usuarioRF, string usuarioNome)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
            UsuarioRF = usuarioRF;
            UsuarioNome = usuarioNome;
        }
    }

    public class NotificacaoEncerramentoEncaminhamentoAEECommandValidator : AbstractValidator<NotificacaoEncerramentoEncaminhamentoAEECommand>
    {
        public NotificacaoEncerramentoEncaminhamentoAEECommandValidator()
        {
            RuleFor(c => c.EncaminhamentoAEEId)
               .NotEmpty()
               .WithMessage("O encaminhamento aee deve ser informado para notificação.");
        }
    }
}
