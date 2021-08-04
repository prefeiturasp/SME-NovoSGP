using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class NotificarUsuarioCommand : IRequest<long>
    {
        public NotificarUsuarioCommand(string titulo,
                                       string mensagem,
                                       string usuarioRf,
                                       NotificacaoCategoria categoria,
                                       NotificacaoTipo tipo,
                                       string dreCodigo = "",
                                       string ueCodigo = "",
                                       string turmaCodigo = "",
                                       int ano = 0,
                                       long codigo = 0, 
                                       DateTime? criadoEm = null,
                                       string nomeUsuario = "",
                                       long usuarioId = 0)
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
            Codigo = codigo;
            CriadoEm = criadoEm;
            UsuarioId = usuarioId;
            NomeUsuario = NomeUsuario;
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
        public long Codigo { get; set; }
        public DateTime? CriadoEm { get; set; }
        public long UsuarioId { get; }
        public string NomeUsuario { get; set; }
    }

    public class NotificarUsuarioCommandValidator : AbstractValidator<NotificarUsuarioCommand>
    {

    }
}
