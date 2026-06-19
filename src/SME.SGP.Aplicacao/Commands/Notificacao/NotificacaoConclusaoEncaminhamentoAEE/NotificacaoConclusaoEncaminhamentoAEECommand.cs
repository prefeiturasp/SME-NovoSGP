using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoConclusaoEncaminhamentoAEECommand : IRequest<bool>
    {
        public long EncaminhamentoAEEId { get; set; }
        public string UsuarioRF { get; set; }
        public string UsuarioNome { get; set; }


        public NotificacaoConclusaoEncaminhamentoAEECommand(long encaminhamentoAEEId, string usuarioRF, string usuarioNome)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
            UsuarioRF = usuarioRF;
            UsuarioNome = usuarioNome;
        }
    }

    public class NotificacaoConclusaoEncaminhamentoAEECommandValidator : AbstractValidator<NotificacaoConclusaoEncaminhamentoAEECommand>
    {
        public NotificacaoConclusaoEncaminhamentoAEECommandValidator()
        {
            RuleFor(c => c.EncaminhamentoAEEId)
               .NotEmpty()
               .WithMessage("O encaminhamento aee deve ser informado para notificação.");
        }
    }
}
