using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class PublicaFilaSgpDto
    {
        public PublicaFilaSgpDto(string nomeFila, object filtros, Guid codigoCorrelacao, Usuario usuarioLogado, bool notificarErroUsuario = false)
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
}
