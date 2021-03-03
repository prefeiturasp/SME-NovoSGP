using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarNotificacaoPlanoAEECommand : IRequest<bool>
    {
        public GerarNotificacaoPlanoAEECommand(long planoId, long usuarioId, string titulo, string descricao, NotificacaoPlanoAEETipo notificacaoPlanoAEETipo)
        {
            PlanoId = planoId;
            UsuarioId = usuarioId;
            Titulo = titulo;
            Descricao = descricao;
            Tipo = notificacaoPlanoAEETipo;
        }

        public long PlanoId { get; }
        public long UsuarioId { get; }
        public string Titulo { get; }
        public string Descricao { get; }
        public NotificacaoPlanoAEETipo Tipo { get; }
    }

    public class GerarNotificacaoPlanoAEECommandValidator : AbstractValidator<GerarNotificacaoPlanoAEECommand>
    {
        public GerarNotificacaoPlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoId)
                .NotEmpty()
                .WithMessage("O id do plano deve ser informado para geração de notificação de plano AEE");

            RuleFor(a => a.UsuarioId)
                .NotEmpty()
                .WithMessage("O id do usuário deve ser informado para geração de notificação de plano AEE");

            RuleFor(a => a.Titulo)
                .NotEmpty()
                .WithMessage("O título deve ser informado para geração de notificação de plano AEE");

            RuleFor(a => a.Descricao)
                .NotEmpty()
                .WithMessage("A descrição deve ser informada para geração de notificação de plano AEE");
        }
    }
}
