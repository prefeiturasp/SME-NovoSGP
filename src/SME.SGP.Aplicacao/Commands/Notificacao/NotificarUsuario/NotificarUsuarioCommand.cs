using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class NotificarUsuarioCommand: IRequest<bool>
    {
        public NotificarUsuarioCommand(string titulo,
                                       string mensagem,
                                       string usuarioRf,
                                       NotificacaoCategoria categoria,
                                       NotificacaoTipo tipo,
                                       string dreCodigo = "",
                                       string ueCodigo = "",
                                       string turmaCodigo = "",
                                       int ano = 0)
        {
            Titulo = titulo;
            Mensagem = mensagem;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            TurmaCodigo = turmaCodigo;
            Ano = ano;
            Categoria = categoria;
            Tipo = tipo;
            UsuarioRf = usuarioRf;
        }

        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public int Ano { get; set; }
        public NotificacaoCategoria Categoria { get; set; }
        public NotificacaoTipo Tipo { get; set; }
        public string UsuarioRf { get; set; }
    }

    public class NotificarUsuarioCommandValidator: AbstractValidator<NotificarUsuarioCommand>
    {

    }
}
