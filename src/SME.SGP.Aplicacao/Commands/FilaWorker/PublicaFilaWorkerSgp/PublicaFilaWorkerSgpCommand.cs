using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaWorkerSgpCommand : IRequest<bool>
    {
        public PublicaFilaWorkerSgpCommand(string nomeFila, object filtros, Guid codigoCorrelacao, Usuario usuarioLogado, bool notificarErroUsuario = false)
        {
            Filtros = filtros;
            CodigoCorrelacao = codigoCorrelacao;
            NotificarErroUsuario = notificarErroUsuario;
            UsuarioLogadoNomeCompleto = usuarioLogado?.Nome;
            UsuarioLogadoRF = usuarioLogado?.CodigoRf;
            PerfilUsuario = usuarioLogado == null ? Guid.Empty : usuarioLogado.PerfilAtual;
            NomeFila = nomeFila;
        }

        public string NomeFila { get; private set; }
        public object Filtros { get; private set; }
        public Guid CodigoCorrelacao { get; private set; }
        public bool NotificarErroUsuario { get; }
        public string UsuarioLogadoNomeCompleto { get; private set; }
        public string UsuarioLogadoRF { get; private set; }
        public Guid PerfilUsuario { get; }
    }

    public class PublicaFilaWorkerSgpCommandValidator : AbstractValidator<PublicaFilaWorkerSgpCommand>
    {
        public PublicaFilaWorkerSgpCommandValidator()
        {
            RuleFor(c => c.NomeFila)
               .NotEmpty()
               .WithMessage("O nome da fila deve ser informado para publicar na fila do worker.");

            RuleFor(c => c.Filtros)
               .NotEmpty()
               .WithMessage("O objeto do filtro ser informado para publicar na fila do worker.");

            RuleFor(c => c.PerfilUsuario)
               .Must(a => a != Guid.Empty)
               .WithMessage("O nome da fila deve ser informado para publicar na fila do worker.");
        }
    }
}
