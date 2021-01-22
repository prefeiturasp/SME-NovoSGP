using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaWorkerServidorRelatoriosCommand : IRequest<bool>
    {
        public PublicaFilaWorkerServidorRelatoriosCommand(string fila, object mensagem, string endpoint, Guid codigoCorrelacao, string codigoRfUsuario, bool notificarErroUsuario = false, string perfilUsuario = null)
        {
            Fila = fila;
            Mensagem = mensagem;
            Endpoint = endpoint;
            CodigoCorrelacao = codigoCorrelacao;
            NotificarErroUsuario = notificarErroUsuario;
            UsuarioLogadoRF = codigoRfUsuario;
            PerfilUsuario = perfilUsuario;
        }

        public string Fila { get; set; }
        public object Mensagem { get; set; }
        public string Endpoint { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public bool NotificarErroUsuario { get; set; }
        public string UsuarioLogadoRF { get; }
        public string PerfilUsuario { get; }
    }

    public class PublicaFilaWorkerServidorRelatoriosCommandValidator : AbstractValidator<PublicaFilaWorkerServidorRelatoriosCommand>
    {
        public PublicaFilaWorkerServidorRelatoriosCommandValidator()
        {
            RuleFor(c => c.Fila)
               .NotEmpty()
               .WithMessage("O nome da fila deve ser informado para publicar na fila de relatórios.");

            RuleFor(c => c.Mensagem)
               .NotEmpty()
               .WithMessage("O objeto da mensagem ser informado para publicar na fila de relatórios.");

            RuleFor(c => c.Endpoint)
               .NotEmpty()
               .WithMessage("O endpoint ser informado para publicar na fila de relatórios.");

            RuleFor(c => c.CodigoCorrelacao)
               .Must(a => a != Guid.Empty)
               .WithMessage("O nome da fila deve ser informado para publicar na fila de relatórios.");
        }
    }
}
