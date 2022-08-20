using FluentValidation;
using System;

namespace SME.SGP.Infra
{
    public class MensagemNotificacaoDto
    {
        public MensagemNotificacaoDto(long codigo, string titulo, DateTime data, string usuarioRf)
        {
            Codigo = codigo;
            Titulo = titulo;
            Data = data;
            UsuarioRf = usuarioRf;
        }

        public long Codigo { get; }
        public string Titulo { get; }
        public DateTime Data { get; }
        public string UsuarioRf { get; }
    }

    public class MensagemNotificacaoDtoValidator : AbstractValidator<MensagemNotificacaoDto>
    {
        public MensagemNotificacaoDtoValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty()
                .WithMessage("O código da notificação deve ser informado para notificar a geração");

            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O título da notificação deve ser informado para notificar a geração");

            RuleFor(x => x.Data)
                .NotEmpty()
                .WithMessage("A data da notificação deve ser informado para notificar a geração");

            RuleFor(x => x.UsuarioRf)
                .NotEmpty()
                .WithMessage("O registro funcional do usuário deve ser informado para notificação");
        }
    }
}
